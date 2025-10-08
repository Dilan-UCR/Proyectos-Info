import asyncio
import json
from aiokafka import AIOKafkaProducer
from app.services.interfaces.i_kafka_service import IKafkaService
from app.models.kafka_log_dto import KafkaLogDTO
from app.core.config import settings

class KafkaService(IKafkaService):
    def __init__(self):
        self.bootstrap_servers = settings.KAFKA_BOOTSTRAP_SERVERS
        self.producer = None
        self.lock = asyncio.Lock()

    async def _get_producer(self):
        async with self.lock:
            if not self.producer:
                self.producer = AIOKafkaProducer(
                    bootstrap_servers=self.bootstrap_servers,
                    value_serializer=lambda v: json.dumps(v).encode("utf-8")
                )
                await self.producer.start()
        return self.producer

    async def send_log(self, log_dto: KafkaLogDTO):
        try:
            producer = await self._get_producer()
            await producer.send_and_wait(
                settings.KAFKA_LOG_TOPIC,
                log_dto.model_dump()
            )
        except Exception as e:
            print(f"[KafkaService] Error al enviar log: {e}")

    async def close(self):
        if self.producer:
            await self.producer.stop()
            self.producer = None
