class AppBaseException(Exception):
    def __init__(self, message: str, status_code: int = 400, error_code: str = "APP_ERROR"):
        self.message = message
        self.status_code = status_code
        self.error_code = error_code
        super().__init__(message)


class NotFoundException(AppBaseException):
    def __init__(self, message: str):
        super().__init__(message, status_code=404, error_code="NOT_FOUND")


class ValidationException(AppBaseException):
    def __init__(self, message: str):
        super().__init__(message, status_code=400, error_code="VALIDATION_ERROR")


class InternalErrorException(AppBaseException):
    def __init__(self, message: str):
        super().__init__(message, status_code=500, error_code="INTERNAL_ERROR")


class StorageServerException(AppBaseException):
    def __init__(self, message: str, status_code: int = 502):
        super().__init__(message, status_code=status_code, error_code="STORAGE_SERVER_ERROR")


class StorageFileNotFoundException(AppBaseException):
    def __init__(self, message: str):
        super().__init__(message, status_code=404, error_code="FILE_NOT_FOUND")


class StorageConnectionException(AppBaseException):
    def __init__(self, message: str):
        super().__init__(message, status_code=503, error_code="STORAGE_CONNECTION_ERROR")


class StorageTimeoutException(AppBaseException):
    def __init__(self, message: str):
        super().__init__(message, status_code=504, error_code="STORAGE_TIMEOUT")


class EmailSendException(AppBaseException):
    def __init__(self, message: str):
        super().__init__(message, status_code=500, error_code="EMAIL_SEND_ERROR")


class EmailValidationException(AppBaseException):
    def __init__(self, message: str):
        super().__init__(message, status_code=400, error_code="EMAIL_VALIDATION_ERROR")


class EmailRecipientRejectedException(AppBaseException):
    def __init__(self, message: str):
        super().__init__(message, status_code=400, error_code="EMAIL_RECIPIENT_REJECTED")


class EmailAuthenticationException(AppBaseException):
    def __init__(self, message: str):
        super().__init__(message, status_code=401, error_code="EMAIL_AUTHENTICATION_ERROR")
