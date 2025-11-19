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
        self.connection_failed = False
        self.last_connection_attempt = 0
        self.retry_interval = 60

    async def _get_producer(self):
        import time
        
        if self.connection_failed:
            if time.time() - self.last_connection_attempt < self.retry_interval:
                return None
        
        async with self.lock:
            if not self.producer and not self.connection_failed:
                temp_producer = None
                try:
                    temp_producer = AIOKafkaProducer(
                        bootstrap_servers=self.bootstrap_servers,
                        value_serializer=lambda v: json.dumps(v).encode("utf-8"),
                        request_timeout_ms=5000,
                        connections_max_idle_ms=60000
                    )
                    await asyncio.wait_for(temp_producer.start(), timeout=5.0)
                    self.connection_failed = False
                    self.producer = temp_producer
                    print(f"[KafkaService] Conexión a Kafka exitosa: {self.bootstrap_servers}")
                except asyncio.TimeoutError:
                    print(f"[KafkaService] Timeout al conectar a Kafka: {self.bootstrap_servers}")
                    self.connection_failed = True
                    self.last_connection_attempt = time.time()
                    if temp_producer:
                        try:
                            await temp_producer.stop()
                        except:
                            pass
                    return None
                except Exception as e:
                    print(f"[KafkaService] Error al conectar a Kafka: {e}")
                    self.connection_failed = True
                    self.last_connection_attempt = time.time()
                    if temp_producer:
                        try:
                            await temp_producer.stop()
                        except:
                            pass
                    return None
        return self.producer

    async def send_log(self, log_dto: KafkaLogDTO):
        if not settings.KAFKA_ENABLED:
            print(f"[KafkaService] Kafka deshabilitado. Log local: [{log_dto.level}] {log_dto.message}")
            return
            
        try:
            producer = await self._get_producer()
            if not producer:
                print(f"[KafkaService] Kafka no disponible. Log no enviado: [{log_dto.level}] {log_dto.message}")
                return
            
            await asyncio.wait_for(
                producer.send_and_wait(settings.KAFKA_LOG_TOPIC, log_dto.model_dump()),
                timeout=3.0
            )
        except asyncio.TimeoutError:
            print(f"[KafkaService] Timeout al enviar log a Kafka")
        except Exception as e:
            print(f"[KafkaService] Error al enviar log: {e}")

    async def close(self):
        if self.producer:
            try:
                await asyncio.wait_for(self.producer.stop(), timeout=5.0)
            except:
                pass
            self.producer = None
