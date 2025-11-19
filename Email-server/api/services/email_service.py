from api.services.interfaces.iemail_service import IEmailService
from api.models.email_model import EmailSendRequest
from api.utils.functions.email_builder import build_email_body
from api.utils.functions.email_sender import build_email_message, send_email_smtp
from api.exceptions.base_exceptions import (EmailSendException, EmailRecipientRejectedException, EmailAuthenticationException)
from api.utils.constants.messages import Messages
from api.utils.static.kafka_methods import KafkaLogger

class EmailService(IEmailService):

    async def send_email(self, email_request: EmailSendRequest, pdf_file: bytes) -> dict:
        
        try:
            body_html = build_email_body(email_request)
            
            msg = build_email_message(email_request, body_html, pdf_file)
            
            # Log 4: Enviando correo al destinatario (no bloqueante)
            KafkaLogger.log_info(
                correlation_id=email_request.correlation_id,
                customer_id=str(email_request.customer_id),
                recipient_email=email_request.recipient_email,
                message=Messages.SENDING_EMAIL_TO_RECIPIENT
            )
            
            await send_email_smtp(msg, email_request.recipient_email)
            
            # Log 4.1: Correo enviado exitosamente (no bloqueante)
            KafkaLogger.log_success(
                correlation_id=email_request.correlation_id,
                customer_id=str(email_request.customer_id),
                recipient_email=email_request.recipient_email,
                message=Messages.EMAIL_SENT_SUCCESSFULLY
            )
            
            return {
                "status": "success",
                "message": Messages.EMAIL_SEND_SUCCESS,
                "correlation_id": email_request.correlation_id
            }
        
        except Exception as e:
            error_message = str(e).lower()
            
            if "email destinatario rechazado" in error_message or "recipients refused" in error_message:
                KafkaLogger.log_error(
                    correlation_id=email_request.correlation_id,
                    customer_id=str(email_request.customer_id),
                    recipient_email=email_request.recipient_email,
                    message=Messages.ERROR_EMAIL_INVALID_RECIPIENT
                )
                raise EmailRecipientRejectedException(Messages.EMAIL_REJECTED_RECIPIENT)
            elif "error de autenticación" in error_message or "authentication" in error_message:
                KafkaLogger.log_error(
                    correlation_id=email_request.correlation_id,
                    customer_id=str(email_request.customer_id),
                    recipient_email=email_request.recipient_email,
                    message=Messages.ERROR_SMTP_CONNECTION
                )
                raise EmailAuthenticationException(Messages.EMAIL_AUTHENTICATION_ERROR)
            else:
                KafkaLogger.log_error(
                    correlation_id=email_request.correlation_id,
                    customer_id=str(email_request.customer_id),
                    recipient_email=email_request.recipient_email,
                    message=Messages.ERROR_SENDING_EMAIL
                )
                raise EmailSendException(Messages.EMAIL_SEND_FAILED)