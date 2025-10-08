import os
from datetime import datetime
from app.core.config import settings

def get_weekly_folder() -> str:

    now = datetime.now()
    year, week, _ = now.isocalendar()
    week_folder = f"{year}-W{week:02d}"

    target_dir = os.path.join(settings.UPLOAD_DIR, week_folder)
    os.makedirs(target_dir, exist_ok=True)

    return target_dir
