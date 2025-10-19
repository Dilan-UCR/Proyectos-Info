from api.services.interfaces.iemail_service import IEmailService
from api.models.email_model import EmailSendRequest
from api.utils.functions.email_builder import build_email_body
from api.utils.functions.email_sender import build_email_message, send_email_smtp
from api.exceptions.base_exceptions import (EmailSendException, EmailRecipientRejectedException, EmailAuthenticationException)
from api.utils.constants.messages import Messages

class EmailService(IEmailService):

    async def send_email(self, email_request: EmailSendRequest, pdf_file: bytes) -> dict:
        
        try:
            body_html = build_email_body(email_request)
            
            msg = build_email_message(email_request, body_html, pdf_file)
            
            await send_email_smtp(msg, email_request.recipient_email)
            
            return {
                "status": "success",
                "message": Messages.EMAIL_SEND_SUCCESS,
                "correlation_id": email_request.correlation_id
            }
        
        except Exception as e:
            error_message = str(e).lower()
            
            if "email destinatario rechazado" in error_message or "recipients refused" in error_message:
                raise EmailRecipientRejectedException(Messages.EMAIL_REJECTED_RECIPIENT)
            elif "error de autenticación" in error_message or "authentication" in error_message:
                raise EmailAuthenticationException(Messages.EMAIL_AUTHENTICATION_ERROR)
            else:
                raise EmailSendException(Messages.EMAIL_SEND_FAILED)