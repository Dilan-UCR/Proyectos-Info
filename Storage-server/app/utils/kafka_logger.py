from fastapi import Depends
from app.services.kafka_service import KafkaService
from app.services.interfaces.i_kafka_service import IKafkaService

_kafka_service_instance = KafkaService()

def get_kafka_service() -> IKafkaService:
    return _kafka_service_instance
