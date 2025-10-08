import os
from fastapi import UploadFile
from app.models.request_dto import RequestDTO
from app.services.interfaces.i_pdf_service import IPdfService
from app.core.config import settings
from app.validations.pdf_validations import validate_pdf_file
from app.validations.request_validations import validate_request_fields
from app.core.messages import Messages
from app.utils.file_weekly_folder import get_weekly_folder
from app.utils.file_naming import generate_filename
from app.utils.file_storage import save_file_to_disk
from app.utils.file_search import find_pdf_by_correlation_id
from app.utils.file_rename import get_clean_filename
from app.validations.correlation_validation import validate_correlation_id
from app.exceptions.base_exceptions import NotFoundException
from app.utils.kafka_log_helper import kafka_log_info, kafka_log_success, kafka_log_error

class PdfService(IPdfService):
    def __init__(self):
        os.makedirs(settings.UPLOAD_DIR, exist_ok=True)

    async def store_pdf(self, file: UploadFile, request_dto: RequestDTO):
        correlation_id = request_dto.correlationId
        
        try:
            # Log de inicio del proceso
            await kafka_log_info(f"Iniciando proceso de subida de PDF para cliente {request_dto.clientId}", correlation_id)
            
            # Validaciones
            validate_pdf_file(file)
            validate_request_fields(request_dto.correlationId, request_dto.clientId, request_dto.dateGeneration)
            await kafka_log_info("Validaciones completadas exitosamente", correlation_id)

            # Obtener carpeta semanal para guardar el pdf
            target_dir = get_weekly_folder()
            
            # Generar nombre de archivo
            filename = generate_filename(request_dto.correlationId, request_dto.dateGeneration)

            # Guardar el archivo
            await save_file_to_disk(file, target_dir, filename)
            
            # Log de éxito
            await kafka_log_success(f"PDF guardado exitosamente: {filename} en {target_dir}", correlation_id)

            return {
                "message": Messages.FILE_SAVED,
                "CorrelationId": request_dto.correlationId
            }
            
        except Exception as e:
            # Log de error
            await kafka_log_error(f"Error al procesar PDF: {str(e)}", correlation_id)
            raise

    async def retrieve_pdf(self, correlation_id: str) -> tuple[str, str]:
        try:
            # Log de inicio de búsqueda
            await kafka_log_info(f"Buscando PDF con correlation_id: {correlation_id}", correlation_id)
            
            # Validar que el correlationId es correcto
            validate_correlation_id(correlation_id)

            # Buscar la ruta del archivo
            file_path = find_pdf_by_correlation_id(correlation_id)
            if not file_path:
                await kafka_log_error(f"PDF no encontrado para correlation_id: {correlation_id}", correlation_id)
                raise NotFoundException(Messages.FILE_NOT_FOUND)

            # Cambiar el nombre
            clean_name = get_clean_filename(file_path, correlation_id)
            
            # Log de éxito
            await kafka_log_success(f"PDF encontrado y recuperado: {clean_name}", correlation_id)

            return file_path, clean_name
            
        except Exception as e:
            # Log de error (solo si no es NotFoundException ya logueada)
            if not isinstance(e, NotFoundException):
                await kafka_log_error(f"Error al recuperar PDF: {str(e)}", correlation_id)
            raise

