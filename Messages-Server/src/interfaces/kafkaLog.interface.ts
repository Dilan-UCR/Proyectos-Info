export interface KafkaLog {
  timestamp?: string;
  type: "success" | "error";
  correlationId: string;
  platform?: string;
  chatId?: string;
  message?: string;
  error?: string;
}

export interface IKafkaService {
  log(type: "success" | "error", data: Omit<KafkaLog, "timestamp" | "type">): Promise<void>;
}