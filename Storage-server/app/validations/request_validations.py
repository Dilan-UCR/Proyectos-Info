from fastapi import HTTPException
from datetime import datetime
from app.core.messages import Messages

def validate_request_fields(correlation_id: str, client_id: int, date_generation: str):
    if not correlation_id or correlation_id.strip() == "":
        raise HTTPException(status_code=400, detail=Messages.MISSING_CORRELATION_ID)

    if not isinstance(client_id, int) or client_id <= 0:
        raise HTTPException(status_code=400, detail=Messages.INVALID_CLIENT_ID)

    try:
        # Solo permite el formato "YYYY-MM-DD"
        date_obj = datetime.strptime(date_generation, "%Y-%m-%d")
    except ValueError:
        raise HTTPException(status_code=400, detail=Messages.INVALID_DATE_FORMAT)

    today = datetime.today()
    ten_years_ago = today.replace(year=today.year - 10)

    if date_obj > today:
        raise HTTPException(status_code=400, detail=Messages.INVALID_DATE_TIME)
    if date_obj < ten_years_ago:
        raise HTTPException(status_code=400, detail=Messages.INVALID_DATE_TIME)
