export interface SendMessageRequest {
  CorrelationId: string;
  ChatId: string;
  Platform: string;
  Message: string;
}