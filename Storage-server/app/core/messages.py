class Messages:
    # estándar
    MISSING_CORRELATION_ID = "El campo 'correlationId' es obligatorio"
    INVALID_CLIENT_ID = "El campo 'clientId' debe ser un número entero positivo"
    INVALID_DATE_FORMAT = "El campo 'dateGeneration' debe tener el siguiente formato: 2025-10-08"
    INVALID_DATE_TIME = "El campo 'dateGeneration' no puede ser una fecha futura o anterior a hace 10 años"



    # errores
    FILE_NOT_FOUND = "Archivo no encontrado"
    INTERNAL_SERVER_ERROR = "Error interno del servidor"
    INVALID_EXTENSION = "Extensión de archivo no permitida"
    FILE_TOO_LARGE = "El archivo excede el tamaño máximo permitido"    
    FILE_NOT_FOUND = "Archivo no encontrado"
    UNEXPECTED_ERROR = "Ocurrió un error interno en el servidor"
    FILE_ALREADY_EXISTS = "El archivo ya existe"

    # para logs
    FILE_SAVED = "PDF almacenado correctamente"
