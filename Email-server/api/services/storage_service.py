import httpx
from api.services.interfaces.istorage_service import IStorageService
from api.core.config import settings
from api.exceptions.base_exceptions import (StorageFileNotFoundException, StorageConnectionException, StorageTimeoutException, StorageServerException)
from api.utils.constants.messages import Messages

class StorageService(IStorageService):
    
    def __init__(self):
        self.base_url = settings.STORAGE_SERVER_URL
        self.timeout = settings.STORAGE_SERVER_TIMEOUT
    
    async def get_file(self, correlation_id: str) -> bytes:

        correlation_id = correlation_id.strip()
        
        url = f"{self.base_url}/{correlation_id}"
        
        try:
            async with httpx.AsyncClient() as client:
                response = await client.get(url, timeout=self.timeout)
                
                if response.status_code == 404:
                    raise StorageFileNotFoundException(Messages.STORAGE_FILE_NOT_FOUND)
                
                response.raise_for_status()
                return response.content
                
        except httpx.TimeoutException:
            raise StorageTimeoutException(Messages.STORAGE_TIMEOUT_ERROR)
            
        except httpx.ConnectError:
            raise StorageConnectionException(Messages.STORAGE_CONNECTION_ERROR)
            
        except httpx.HTTPStatusError as e:
            if e.response.status_code == 404:
                raise StorageFileNotFoundException(Messages.STORAGE_FILE_NOT_FOUND)
            else:
                raise StorageServerException(f"Error HTTP {e.response.status_code} del Storage Server")
                
        except (StorageFileNotFoundException, StorageTimeoutException, StorageConnectionException, StorageServerException):
            raise
        except Exception as e:
            raise StorageServerException(f"Error inesperado del Storage Server: {str(e)}")
        