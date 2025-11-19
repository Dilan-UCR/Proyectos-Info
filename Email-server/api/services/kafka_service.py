import json
import asyncio
from typing import Optional
from aiokafka import AIOKafkaProducer
from aiokafka.errors import KafkaConnectionError, KafkaTimeoutError
from api.services.interfaces.ikafka_service import IKafkaService
from api.models.kafka_model import kafkaLog
from api.core.config import settings

class KafkaService(IKafkaService):
    
    def __init__(self):
        self.bootstrap_servers = settings.KAFKA_BOOTSTRAP_SERVERS
        self.producer = None
        self.lock = asyncio.Lock()
        self.connection_failed = False
        self.connection_timeout = 5  # timeout en segundos
    
    async def get_producer(self):
        # Si ya falló la conexión anteriormente, no intentar de nuevo
        if self.connection_failed:
            return None
            
        async with self.lock:
            if not self.producer:
                try:
                    self.producer = AIOKafkaProducer(
                        bootstrap_servers=self.bootstrap_servers,
                        value_serializer=lambda v: json.dumps(v).encode("utf-8"),
                        request_timeout_ms=5000,  # 5 segundos
                        connections_max_idle_ms=10000
                    )
                    # Intentar conectar con timeout
                    await asyncio.wait_for(
                        self.producer.start(),
                        timeout=self.connection_timeout
                    )
                except (KafkaConnectionError, KafkaTimeoutError, asyncio.TimeoutError, Exception) as e:
                    print(f"[KafkaService] No se pudo conectar a Kafka: {e}")
                    self.connection_failed = True
                    self.producer = None
                    return None
        return self.producer
    
    async def send_log(self, kafka_log:kafkaLog):
        try:
            producer = await asyncio.wait_for(
                self.get_producer(),
                timeout=self.connection_timeout
            )
            
            if producer is None:
                print(f"[KafkaService] Kafka no disponible. Log no enviado: {kafka_log.message}")
                return
            
            await asyncio.wait_for(
                producer.send_and_wait(
                    settings.KAFKA_LOG_TOPIC,
                    kafka_log.model_dump()
                ),
                timeout=3  # 3 segundos para enviar
            )
        except asyncio.TimeoutError:
            print(f"[KafkaService] Timeout al enviar log: {kafka_log.message}")
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
