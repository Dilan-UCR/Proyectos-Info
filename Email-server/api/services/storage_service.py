import httpx
from api.services.interfaces.istorage_service import IStorageService
from api.core.config import settings
from api.exceptions.base_exceptions import (StorageFileNotFoundException, StorageConnectionException, StorageTimeoutException, StorageServerException)
from api.utils.constants.messages import Messages
from api.utils.static.kafka_methods import KafkaLogger

class StorageService(IStorageService):
    
    def __init__(self):
        self.base_url = settings.STORAGE_SERVER_URL
        self.timeout = settings.STORAGE_SERVER_TIMEOUT
    
    async def get_file(self, correlation_id: str, customer_id: str, recipient_email: str) -> bytes:

        correlation_id = correlation_id.strip()
        
        # Log 2: Solicitando archivo al Storage Server (no bloqueante)
        KafkaLogger.log_info(
            correlation_id=correlation_id,
            customer_id=customer_id,
            recipient_email=recipient_email,
            message=Messages.REQUESTING_FILE_FROM_STORAGE
        )
        
        url = f"{self.base_url}/{correlation_id}"
        
        try:
            async with httpx.AsyncClient() as client:
                response = await client.get(url, timeout=self.timeout)
                
                if response.status_code == 404:
                    # Log error: Archivo no encontrado (no bloqueante)
                    KafkaLogger.log_error(
                        correlation_id=correlation_id,
                        customer_id=customer_id,
                        recipient_email=recipient_email,
                        message=Messages.ERROR_FILE_NOT_FOUND
                    )
                    raise StorageFileNotFoundException(Messages.STORAGE_FILE_NOT_FOUND)
                
                response.raise_for_status()
                
                # Log 3: Archivo recuperado correctamente (no bloqueante)
                KafkaLogger.log_success(
                    correlation_id=correlation_id,
                    customer_id=customer_id,
                    recipient_email=recipient_email,
                    message=Messages.FILE_RETRIEVED_SUCCESSFULLY
                )
                
                return response.content
                
        except httpx.TimeoutException:
            KafkaLogger.log_error(
                correlation_id=correlation_id,
                customer_id=customer_id,
                recipient_email=recipient_email,
                message=Messages.ERROR_STORAGE_TIMEOUT
            )
            raise StorageTimeoutException(Messages.STORAGE_TIMEOUT_ERROR)
            
        except httpx.ConnectError:
            KafkaLogger.log_error(
                correlation_id=correlation_id,
                customer_id=customer_id,
                recipient_email=recipient_email,
                message=Messages.ERROR_STORAGE_CONNECTION
            )
            raise StorageConnectionException(Messages.STORAGE_CONNECTION_ERROR)
            
        except httpx.HTTPStatusError as e:
            if e.response.status_code == 404:
                KafkaLogger.log_error(
                    correlation_id=correlation_id,
                    customer_id=customer_id,
                    recipient_email=recipient_email,
                    message=Messages.ERROR_FILE_NOT_FOUND
                )
                raise StorageFileNotFoundException(Messages.STORAGE_FILE_NOT_FOUND)
            else:
                KafkaLogger.log_error(
                    correlation_id=correlation_id,
                    customer_id=customer_id,
                    recipient_email=recipient_email,
                    message=f"Error HTTP {e.response.status_code} del Storage Server"
                )
                raise StorageServerException(f"Error HTTP {e.response.status_code} del Storage Server")
                
        except (StorageFileNotFoundException, StorageTimeoutException, StorageConnectionException, StorageServerException):
            raise
        except Exception as e:
            KafkaLogger.log_error(
                correlation_id=correlation_id,
                customer_id=customer_id,
                recipient_email=recipient_email,
                message=f"Error inesperado del Storage Server: {str(e)}"
            )
            raise StorageServerException(f"Error inesperado del Storage Server: {str(e)}")
        