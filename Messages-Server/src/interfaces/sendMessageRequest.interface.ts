export interface SendMessageRequest {
  correlationId: string;
  chatId: string;
  platform: string;
  message: string;
}