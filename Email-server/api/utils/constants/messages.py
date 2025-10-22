class Messages:
    
    # Mensajes de error generales
    UNEXPECTED_ERROR = "Ha ocurrido un error inesperado en el servidor"
    VALIDATION_ERROR = "Error de validación en los datos proporcionados"
    NOT_FOUND_ERROR = "El recurso solicitado no fue encontrado"
    INTERNAL_ERROR = "Error interno del servidor"
    
    # Mensajes específicos de email
    EMAIL_SEND_SUCCESS = "Email enviado exitosamente"
    EMAIL_SEND_FAILED = "Error al enviar el email"
    EMAIL_INVALID_RECIPIENT = "La dirección de email del destinatario no es válida o fue rechazada"
    EMAIL_ATTACHMENT_ERROR = "Error al procesar el archivo adjunto"
    EMAIL_AUTHENTICATION_ERROR = "Error de autenticación con el servidor de correo"
    EMAIL_REJECTED_RECIPIENT = "El servidor de correo rechazó la dirección del destinatario"
    
    # Mensajes específicos de Storage Server
    STORAGE_FILE_NOT_FOUND = "No se pudo encontrar el archivo en el Storage Server"
    STORAGE_CONNECTION_ERROR = "Error de conexión con el Storage Server"
    STORAGE_TIMEOUT_ERROR = "Tiempo agotado al comunicarse con el Storage Server"
    
    # Mensajes específicos de Kafka
    KAFKA_CONNECTION_ERROR = "Error de conexión con Kafka"
    KAFKA_SEND_ERROR = "Error al enviar mensaje a Kafka"
    
    # Mensajes de validación
    CORRELATION_ID_REQUIRED = "El correlation_id es requerido"
    RECIPIENT_EMAIL_REQUIRED = "El email del destinatario es requerido"

    # Mensajes de operaciones exitosas para kafka
    REQUEST_RECEIVED = "Solicitud de envío de correo recibida"
    REQUESTING_FILE_FROM_STORAGE = "Solicitando archivo al Storage Server"
    FILE_RETRIEVED_SUCCESSFULLY = "Archivo recuperado correctamente del Storage Server"
    SENDING_EMAIL_TO_RECIPIENT = "Enviando correo al destinatario"
    EMAIL_SENT_SUCCESSFULLY = "Correo enviado correctamente al destinatario"
    
    # Mensajes de errores para kafka
    ERROR_REQUESTING_FILE = "Error al solicitar archivo del Storage Server"
    ERROR_FILE_NOT_FOUND = "Archivo no encontrado en el Storage Server"
    ERROR_FILE_RETRIEVAL = "Error al recuperar archivo del Storage Server"
    ERROR_STORAGE_CONNECTION = "Error de conexión con el Storage Server"
    ERROR_STORAGE_TIMEOUT = "Tiempo agotado al comunicarse con el Storage Server"
    ERROR_SENDING_EMAIL = "Error al enviar correo al destinatario"
    ERROR_EMAIL_INVALID_RECIPIENT = "Dirección de email del destinatario inválida"
    ERROR_EMAIL_ATTACHMENT = "Error al procesar archivo adjunto"
    ERROR_SMTP_CONNECTION = "Error de conexión SMTP"
    ERROR_EMAIL_CONSTRUCTION = "Error al construir el correo electrónico"
