from abc import ABC, abstractmethod
from api.models.email_model import EmailSendRequest

class IEmailService(ABC):
    
    @abstractmethod
    async def send_email(self, email_request: EmailSendRequest, pdf_file: bytes) -> dict:
        pass