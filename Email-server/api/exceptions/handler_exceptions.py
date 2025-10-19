from fastapi import Request
from fastapi.responses import JSONResponse
import httpx
from api.exceptions.base_exceptions import AppBaseException
from api.utils.constants.messages import Messages

def register_exception_handlers(app):
    @app.exception_handler(AppBaseException)
    async def app_exception_handler(request: Request, exc: AppBaseException):
        return JSONResponse(
            status_code=exc.status_code,
            content={
                "success": False,
                "error_code": exc.error_code,
                "error_message": exc.message,
                "status_code": exc.status_code,
                "path": str(request.url)
            },
        )

    @app.exception_handler(Exception)
    async def unhandled_exception_handler(request: Request, exc: Exception):
        return JSONResponse(
            status_code=500,
            content={
                "success": False,
                "error_code": "INTERNAL_ERROR",
                "error_message": Messages.UNEXPECTED_ERROR,
                "status_code": 500,
                "path": str(request.url)
            },
        )
