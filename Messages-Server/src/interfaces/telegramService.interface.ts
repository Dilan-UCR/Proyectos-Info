export interface ITelegramService {
  sendFile(chatId: string, fileBuffer: Buffer, caption: string): Promise<void>;
}