import os
from fastapi import UploadFile, HTTPException
from app.core.config import settings
from app.core.messages import Messages

def validate_pdf_file(file: UploadFile):
    _, ext = os.path.splitext(file.filename)
    if ext.lower() not in settings.ALLOWED_EXTENSIONS:
        raise HTTPException(status_code=400, detail=Messages.INVALID_FILE_TYPE)

    file.file.seek(0, os.SEEK_END)
    size = file.file.tell()
    file.file.seek(0)
    if size > settings.MAX_FILE_SIZE:
        raise HTTPException(status_code=400, detail=Messages.FILE_TOO_LARGE)
