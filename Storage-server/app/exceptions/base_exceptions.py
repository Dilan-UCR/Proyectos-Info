class AppBaseException(Exception):
    def __init__(self, message: str, status_code: int = 400):
        self.message = message
        self.status_code = status_code
        super().__init__(message)


class NotFoundException(AppBaseException):
    def __init__(self, message: str):
        super().__init__(message, status_code=404)


class ValidationException(AppBaseException):
    def __init__(self, message: str):
        super().__init__(message, status_code=400)


class InternalErrorException(AppBaseException):
    def __init__(self, message: str):
        super().__init__(message, status_code=500)
