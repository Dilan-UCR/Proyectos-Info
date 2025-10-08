from fastapi import FastAPI
from app.controllers import pdf_controller
from app.exceptions.handler import register_exception_handlers
from app.core.config import settings
from app.utils.kafka_logger import get_kafka_service

app = FastAPI(
    title=settings.APP_NAME,
    version=settings.APP_VERSION
)

app.include_router(pdf_controller.router)
register_exception_handlers(app)

@app.on_event("shutdown")
async def shutdown_event():
    kafka_service = get_kafka_service()
    await kafka_service.close()