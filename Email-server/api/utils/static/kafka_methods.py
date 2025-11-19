import asyncio
from typing import Optional
from api.models.kafka_model import kafkaLog
from api.services.kafka_service import get_kafka_service

class KafkaLogger:
    
    @staticmethod
    def log_info(correlation_id:str, customer_id: str, recipient_email:str, message: str):
        """Envía log de manera no bloqueante"""
        asyncio.create_task(
            KafkaLogger._send_log("INFO", correlation_id, customer_id, recipient_email, message)
        )
    
    @staticmethod
    def log_warning(correlation_id:str, customer_id: str, recipient_email:str, message: str):
        """Envía log de manera no bloqueante"""
        asyncio.create_task(
            KafkaLogger._send_log("WARNING", correlation_id, customer_id, recipient_email, message)
        )
    
    @staticmethod
    def log_error(correlation_id:str, customer_id: str, recipient_email:str, message: str):
        """Envía log de manera no bloqueante"""
        asyncio.create_task(
            KafkaLogger._send_log("ERROR", correlation_id, customer_id, recipient_email, message)
        )
    
    @staticmethod
    def log_success(correlation_id:str, customer_id: str, recipient_email:str, message: str):
        """Envía log de manera no bloqueante"""
        asyncio.create_task(
            KafkaLogger._send_log("SUCCESS", correlation_id, customer_id, recipient_email, message)
        )
    
    @staticmethod
    async def _send_log(level: str, correlation_id:str, customer_id: str, recipient_email:str, message: str):
        try:
            kafka_service = get_kafka_service()
            log_dto = kafkaLog(
                level=level,
                correlation_id=correlation_id,
                customer_id=customer_id,
                recipient_email=recipient_email,
                message=message
            )
            await kafka_service.send_log(log_dto)
        except Exception as e:
            # Silencioso para no afectar el flujo principal
            pass
