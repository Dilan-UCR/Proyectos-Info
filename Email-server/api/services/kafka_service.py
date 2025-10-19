import json
import asyncio
from typing import Optional
from aiokafka import AIOKafkaProducer
from api.services.interfaces.ikafka_service import IKafkaService
from api.models.kafka_model import kafkaLog
from api.core.config import settings

class KafkaService(IKafkaService):
    
    def __init__(self):
        self.bootstrap_servers = settings.KAFKA_BOOTSTRAP_SERVERS
        self.producer = None
        self.lock = asyncio.Lock()
    
    async def get_producer(self):
        async with self.lock:
            if not self.producer:
                self.producer = AIOKafkaProducer(
                    bootstrap_servers=self.bootstrap_servers,
                    value_serializer=lambda v: json.dumps(v).encode("utf-8")
                )
                await self.producer.start()
        return self.producer
    
    async def send_log(self, kafka_log:kafkaLog):
        try:
            producer = await self.get_producer()
            await producer.send_and_wait(
                settings.KAFKA_LOG_TOPIC,
                kafka_log.model_dump()
            )
        except Exception as e:
            print(f"[KafkaService] Error al enviar log: {e}")
        
    async def close(self):
        if self.producer:
            try:
                await self.producer.stop()
                self.producer = None
            except Exception:
                pass
    

# Singleton
_kafka_service_instance: Optional[KafkaService] = None

def get_kafka_service() -> KafkaService:
    global _kafka_service_instance
    if _kafka_service_instance is None:
        _kafka_service_instance = KafkaService()
    return _kafka_service_instance
