from fastapi import Request
from fastapi.responses import JSONResponse
from app.exceptions.base_exceptions import AppBaseException
from app.core.messages import Messages

def register_exception_handlers(app):
    @app.exception_handler(AppBaseException)
    async def app_exception_handler(request: Request, exc: AppBaseException):
        return JSONResponse(
            status_code=exc.status_code,
            content={
                "error": exc.message,
                "status_code": exc.status_code,
                "path": str(request.url)
            },
        )

    @app.exception_handler(Exception)
    async def unhandled_exception_handler(request: Request, exc: Exception):
        return JSONResponse(
            status_code=500,
            content={
                "error": Messages.UNEXPECTED_ERROR,
                "details": str(exc)
            },
        )
