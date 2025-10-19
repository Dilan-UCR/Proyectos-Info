from datetime import datetime
from api.models.email_model import EmailSendRequest

def build_email_body(email_request: EmailSendRequest) -> str:
    
    current_date = datetime.utcnow().strftime("%d de %B de %Y")
    current_time = datetime.utcnow().strftime("%H:%M:%S UTC")
    
    body_html = f"""
    <html>
    <body style="font-family: Arial, sans-serif; margin: 20px;">
        <h2 style="color: #2c3e50;">Reporte Generado</h2>
        
        <p>Estimado usuario,</p>
        
        <p>Su reporte ha sido generado exitosamente y se adjunta a este correo electrónico.</p>
        
        <div style="background-color: #f8f9fa; padding: 15px; border-radius: 5px; margin: 20px 0;">
            <h3 style="color: #495057; margin-top: 0;">Detalles del Reporte:</h3>
            <ul style="list-style-type: none; padding: 0;">
                <li><strong>Título:</strong> {email_request.title}</li>
                <li><strong>Descripción:</strong> {email_request.description}</li>
                <li><strong>ID de Correlación:</strong> {email_request.correlation_id}</li>
                <li><strong>Cliente ID:</strong> {email_request.customer_id}</li>
                <li><strong>Fecha de Generación:</strong> {current_date}</li>
                <li><strong>Hora de Envío:</strong> {current_time}</li>
            </ul>
        </div>
        
        <p>El archivo PDF adjunto contiene toda la información solicitada.</p>
        
        <hr style="border: none; border-top: 1px solid #dee2e6; margin: 30px 0;">
        
        <p style="color: #6c757d; font-size: 12px;">
            Este es un mensaje automático del sistema de reportes. Por favor no responda a este correo.
        </p>
    </body>
    </html>
    """
    
    return body_html