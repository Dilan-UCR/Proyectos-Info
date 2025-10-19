export function logInfo(message: string, data: Record<string, any> = {}) {
  console.log(`[INFO] ${new Date().toISOString()} - ${message}`);
  if (Object.keys(data).length > 0) console.log(data);
}

export function logError(message: string, data: Record<string, any> = {}) {
  console.error(`[ERROR] ${new Date().toISOString()} - ${message}`);
  if (Object.keys(data).length > 0) console.error(data);
}