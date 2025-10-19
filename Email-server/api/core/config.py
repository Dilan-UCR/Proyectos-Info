from pydantic_settings import BaseSettings
import os
from typing import Optional

class Settings(BaseSettings):
    
    APP_NAME: str
    APP_VERSION: str
    SERVICE_NAME: str
    
    HOST: str
    PORT: int

    SMTP_SERVER: Optional[str] = None
    SMTP_PORT: Optional[int] = None
    SMTP_USERNAME: Optional[str] = None
    SMTP_PASSWORD: Optional[str] = None
    SMTP_USE_TLS:bool = False
    SMTP_USE_SSL:bool = False
    
    FROM_EMAIL: Optional[str] = None
    FROM_NAME: Optional[str] = None
    EMAIL_SUBJECT_PREFIX: str = "[Email Service]"
    
    KAFKA_BOOTSTRAP_SERVERS: str = os.getenv("KAFKA_BOOTSTRAP_SERVERS", "localhost:9092")
    KAFKA_LOG_TOPIC: str = os.getenv("KAFKA_LOG_TOPIC", "logs-email")
    KAFKA_SECURITY_PROTOCOL: str

    STORAGE_SERVER_URL: str
    STORAGE_SERVER_TIMEOUT: int
    
    class Config:
        env_file = ".env"

settings = Settings()