from abc import ABC, abstractmethod
from api.models.kafka_model import kafkaLog


class IKafkaService(ABC):
    
    @abstractmethod
    async def get_producer(self):
        pass

    @abstractmethod
    async def send_log(self, kafka_log:kafkaLog):
        pass
    
    @abstractmethod
    async def close(self):
        pass
