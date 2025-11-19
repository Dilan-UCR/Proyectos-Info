from typing import Optional
from app.models.kafka_log_dto import KafkaLogDTO
from app.utils.kafka_logger import get_kafka_service

class KafkaLogger:
    
    @staticmethod
    async def log_info(message: str, correlation_id: Optional[str] = None):
        await KafkaLogger._send_log("INFO", message, correlation_id)
    
    @staticmethod
    async def log_warning(message: str, correlation_id: Optional[str] = None):
        await KafkaLogger._send_log("WARNING", message, correlation_id)
    
    @staticmethod
    async def log_error(message: str, correlation_id: Optional[str] = None):
        await KafkaLogger._send_log("ERROR", message, correlation_id)
    
    @staticmethod
    async def log_success(message: str, correlation_id: Optional[str] = None):
        await KafkaLogger._send_log("SUCCESS", message, correlation_id)
    
    @staticmethod
    async def _send_log(level: str, message: str, correlation_id: Optional[str] = None):
        try:
            kafka_service = get_kafka_service()
            log_dto = KafkaLogDTO(
                level=level,
                message=message,
                correlation_id=correlation_id
            )
            # Enviar de forma no bloqueante con timeout
            import asyncio
            await asyncio.wait_for(kafka_service.send_log(log_dto), timeout=3.0)
        except asyncio.TimeoutError:
            print(f"[KafkaLogger] Timeout al enviar log a Kafka")
            print(f"[KafkaLogger] Log: [{level}] {message} (CorrelationId: {correlation_id})")
        except Exception as e:
            print(f"[KafkaLogger] Error al enviar log a Kafka: {e}")
            print(f"[KafkaLogger] Log que falló: [{level}] {message} (CorrelationId: {correlation_id})")

async def kafka_log_info(message: str, correlation_id: Optional[str] = None):
    """Función de conveniencia para logs INFO."""
    await KafkaLogger.log_info(message, correlation_id)

async def kafka_log_warning(message: str, correlation_id: Optional[str] = None):
    """Función de conveniencia para logs WARNING."""
    await KafkaLogger.log_warning(message, correlation_id)

async def kafka_log_error(message: str, correlation_id: Optional[str] = None):
    """Función de conveniencia para logs ERROR."""
    await KafkaLogger.log_error(message, correlation_id)

async def kafka_log_success(message: str, correlation_id: Optional[str] = None):
    """Función de conveniencia para logs SUCCESS."""
    await KafkaLogger.log_success(message, correlation_id)
