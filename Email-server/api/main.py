from fastapi import FastAPI
from api.exceptions.handler_exceptions import register_exception_handlers
from api.controllers.email_controller import router as email_router
from api.core.config import settings
from api.services.kafka_service import get_kafka_service

app = FastAPI(
    tittle="Email Server API",
    version=settings.APP_VERSION
)

app.include_router(email_router)
register_exception_handlers(app)

@app.on_event("shutdown")
async def shutdown_event():
    kafka_service = get_kafka_service()
    await kafka_service.close()

