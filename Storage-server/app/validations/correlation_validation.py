from fastapi import HTTPException
from app.core.messages import Messages
from app.exceptions.base_exceptions import ValidationException

def validate_correlation_id(correlation_id: str):
    cleaned_id = correlation_id.strip() if correlation_id else ""
    if not cleaned_id:
        raise ValidationException(Messages.MISSING_CORRELATION_ID)
