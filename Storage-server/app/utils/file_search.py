import os
from app.core.config import settings

def find_pdf_by_correlation_id(correlation_id: str) -> str | None:

    for root, _, files in os.walk(settings.UPLOAD_DIR):
        
        for f in files:
            if f.startswith(f"{correlation_id}_"):
                full_path = os.path.join(root, f)
                return full_path
    
    return None
