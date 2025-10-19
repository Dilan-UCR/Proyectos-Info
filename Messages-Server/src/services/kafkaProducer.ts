import { Kafka } from "kafkajs";
import { IKafkaService, KafkaLog } from "../interfaces/kafkaLog.interface";

export class KafkaProducer implements IKafkaService {
  private kafka = new Kafka({ 
    clientId: "messages-server", 
    brokers: [process.env.KAFKA_BROKER!] 
  });
  private producer = this.kafka.producer();
  private topic = process.env.KAFKA_TOPIC!;

  async log(type: "success" | "error", data: Omit<KafkaLog, "timestamp" | "type">) {
    await this.producer.connect();
    await this.producer.send({
      topic: this.topic,
      messages: [
        {
          value: JSON.stringify({
            timestamp: new Date().toISOString(),
            type,
            ...data,
          }),
        },
      ],
    });
    await this.producer.disconnect();
  }
}