import os

def get_clean_filename(file_path: str, correlation_id: str) -> str:
    
    filename = os.path.basename(file_path)
    if filename.startswith(f"{correlation_id}_"):
        return filename[len(correlation_id) + 1:]
    return filename
