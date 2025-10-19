import { StorageService } from "../services/storageService";
import { TelegramService } from "../services/telegramService";
import { KafkaProducer } from "../services/kafkaProducer";

export const storageService = new StorageService();
export const telegramService = new TelegramService(process.env.TELEGRAM_BOT_TOKEN!);
export const kafkaService = new KafkaProducer();