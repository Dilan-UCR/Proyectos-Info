from abc import ABC, abstractmethod

class IStorageService(ABC):
    
    @abstractmethod
    async def get_file(self, correlation_id: str) -> bytes:
        pass