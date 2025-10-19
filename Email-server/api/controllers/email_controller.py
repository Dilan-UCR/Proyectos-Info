from fastapi import APIRouter, status, Depends
from api.models.email_model import EmailSendRequest
from api.services.email_service import EmailService
from api.services.storage_service import StorageService

router = APIRouter(
    prefix="/api/email",
    tags=["Email"]
)

def get_email_service() -> EmailService:
    return EmailService()

def get_storage_service() -> StorageService:
    return StorageService()

@router.post("/send", status_code=status.HTTP_200_OK)
async def send_email(
    request: EmailSendRequest,
    email_service: EmailService = Depends(get_email_service),
    storage_service: StorageService = Depends(get_storage_service)
):
    
    pdf_file = await storage_service.get_file(request.correlation_id)
    
    response = await email_service.send_email(request, pdf_file)
    
    return response