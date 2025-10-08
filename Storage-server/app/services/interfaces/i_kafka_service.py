from abc import ABC, abstractmethod
from app.models.kafka_log_dto import KafkaLogDTO

class IKafkaService(ABC):
    @abstractmethod
    async def send_log(self, log_dto: KafkaLogDTO):
        pass
