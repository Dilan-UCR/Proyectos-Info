from pydantic import BaseModel, Field
from typing import Optional
from datetime import datetime

class KafkaLogDTO(BaseModel):
    level: str
    message: str
    correlation_id: Optional[str] = None
    timestamp: str = Field(default_factory=lambda: datetime.utcnow().isoformat())
    
    class Config:
        json_encoders = {
            datetime: lambda v: v.isoformat()
        }
