from pydantic import BaseModel, EmailStr, Field
from typing import Union

class EmailSendRequest(BaseModel):
    correlation_id: str = Field(alias="CorrelationId")
    recipient_email: EmailStr = Field(alias="ToEmail")
    title: str = Field(alias="Subject")
    description: str = Field(alias="Message")
    customer_id: Union[str, int] = Field(alias="CustomerId")
    
    class Config:
        allow_population_by_field_name = True
