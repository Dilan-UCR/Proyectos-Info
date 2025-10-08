from fastapi import APIRouter, UploadFile, File, Form, Depends
from fastapi.responses import FileResponse
from app.models.request_dto import RequestDTO
from app.services.pdf_service import PdfService

router = APIRouter(prefix="/app/pdf", tags=["PDF"])

def get_pdf_service() -> PdfService:
    return PdfService()

@router.post("/upload", status_code=201)
async def upload_pdf(
    file: UploadFile = File(...),
    correlationId: str = Form(...),
    clientId: int = Form(...),
    dateGeneration: str = Form(...),
    pdf_service: PdfService = Depends(get_pdf_service)
):
    dto = RequestDTO(
        correlationId=correlationId,
        clientId=clientId,
        dateGeneration=dateGeneration
    )

    return await pdf_service.store_pdf(file, dto)

@router.get("/{correlation_id}", status_code=200)
async def get_pdf(
    correlation_id: str,
    pdf_service: PdfService = Depends(get_pdf_service)
):
    file_path, clean_name = await pdf_service.retrieve_pdf(correlation_id)

    return FileResponse(
        path=file_path,
        media_type="application/pdf",
        filename=clean_name
    )