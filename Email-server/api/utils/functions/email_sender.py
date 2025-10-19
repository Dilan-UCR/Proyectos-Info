import smtplib
from email.mime.multipart import MIMEMultipart
from email.mime.text import MIMEText
from email.mime.application import MIMEApplication
from datetime import datetime
from api.models.email_model import EmailSendRequest
from api.core.config import settings

def build_email_message(email_request: EmailSendRequest, body_html: str, pdf_file: bytes) -> MIMEMultipart:
    msg = MIMEMultipart()
    
    msg['From'] = f"{settings.FROM_NAME} <{settings.FROM_EMAIL}>"
    msg['To'] = email_request.recipient_email
    msg['Subject'] = f"{settings.EMAIL_SUBJECT_PREFIX} {email_request.title}"
    msg['Date'] = datetime.utcnow().strftime("%a, %d %b %Y %H:%M:%S +0000")
    
    msg.attach(MIMEText(body_html, 'html'))
    
    pdf_attachment = create_pdf_attachment(pdf_file, email_request.correlation_id)
    msg.attach(pdf_attachment)
    
    return msg

def create_pdf_attachment(pdf_file: bytes, correlation_id: str) -> MIMEApplication:
    pdf_attachment = MIMEApplication(pdf_file, _subtype='pdf')
    filename = f"reporte_{correlation_id}.pdf"
    pdf_attachment.add_header('Content-Disposition', 'attachment', filename=filename)
    
    return pdf_attachment

async def send_email_smtp(msg: MIMEMultipart, recipient: str) -> None:

    server = smtplib.SMTP(settings.SMTP_SERVER, settings.SMTP_PORT)

    try:
        if settings.SMTP_USE_TLS:
            server.starttls()
        
        if settings.SMTP_USERNAME and settings.SMTP_PASSWORD:
            server.login(settings.SMTP_USERNAME, settings.SMTP_PASSWORD)
        
        server.send_message(msg)
        
    except smtplib.SMTPRecipientsRefused as e:
        raise Exception(f"Email destinatario rechazado por el servidor: {recipient}")
        
    except smtplib.SMTPAuthenticationError as e:
        raise Exception(f"Error de autenticación SMTP: Verificar credenciales de Gmail")
        
    except smtplib.SMTPException as e:
        raise Exception(f"Error SMTP: {str(e)}")
        
    finally:
        server.quit()