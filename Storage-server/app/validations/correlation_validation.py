from fastapi import HTTPException
from app.core.messages import Messages
from app.exceptions.base_exceptions import ValidationException

def validate_correlation_id(correlation_id: str):
    if not correlation_id or correlation_id.strip() == "":
        raise ValidationException(Messages.MISSING_CORRELATION_ID)
