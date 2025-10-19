from pydantic import BaseModel, Field
from datetime import datetime

class kafkaLog(BaseModel):
    level: str
    correlation_id: str
    customer_id: str
    recipient_email: str
    message: str
    date: str = Field(default_factory=lambda: datetime.now().strftime("%Y-%m-%d"))
    time: str = Field(default_factory=lambda: datetime.now().strftime("%H:%M:%S"))
