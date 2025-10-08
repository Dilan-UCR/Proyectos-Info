import os
from fastapi import UploadFile

async def save_file_to_disk(file: UploadFile, target_dir: str, filename: str) -> str:
    file_path = os.path.join(target_dir, filename)
    with open(file_path, "wb") as f:
        f.write(await file.read())
    return file_path
