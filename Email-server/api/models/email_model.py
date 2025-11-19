from pydantic import BaseModel, EmailStr, Field, ConfigDict
from typing import Union

class EmailSendRequest(BaseModel):
    model_config = ConfigDict(populate_by_name=True)
    
    correlation_id: str = Field(alias="CorrelationId")
    recipient_email: EmailStr = Field(alias="ToEmail")
    title: str = Field(alias="Subject")
    description: str = Field(alias="Message")
    customer_id: Union[str, int] = Field(alias="CustomerId")
