import os
from pydantic_settings import BaseSettings
from typing import List

class Settings(BaseSettings):
    APP_NAME: str
    APP_VERSION: str
    SERVICE_NAME: str

    HOST: str
    PORT: int

    UPLOAD_DIR: str
    MAX_FILE_SIZE: int
    ALLOWED_EXTENSIONS: List[str]

    KAFKA_ENABLED: bool = os.getenv("KAFKA_ENABLED", "true").lower() == "true"
    KAFKA_SECURITY_PROTOCOL: str
    KAFKA_BOOTSTRAP_SERVERS: str = os.getenv("KAFKA_BOOTSTRAP_SERVERS", "localhost:9092")
    KAFKA_LOG_TOPIC: str = os.getenv("KAFKA_LOG_TOPIC", "logs-storage")

    class Config:
        env_file = ".env"

settings = Settings()
