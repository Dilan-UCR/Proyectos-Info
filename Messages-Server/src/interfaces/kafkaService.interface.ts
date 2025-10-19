import { KafkaLog } from "./kafkaLog.interface";

export interface IKafkaService {
  log(type: "success" | "error", data: Omit<KafkaLog, "timestamp" | "type">): Promise<void>;
}