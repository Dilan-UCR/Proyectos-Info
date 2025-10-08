from abc import ABC, abstractmethod
from fastapi import UploadFile
from app.models.request_dto import RequestDTO

class IPdfService(ABC):
    @abstractmethod
    async def store_pdf(self, file: UploadFile, request_dto: RequestDTO):
        pass

    @abstractmethod
    async def retrieve_pdf(self, correlation_id: str) -> str:
        pass
