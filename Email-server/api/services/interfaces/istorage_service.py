from abc import ABC, abstractmethod

class IStorageService(ABC):
    
    @abstractmethod
    async def get_file(self, correlation_id: str, customer_id: str, recipient_email: str) -> bytes:
        pass