import { Router } from "express";
import { createMessagingController } from "../controllers/messagingController";
import { storageService, telegramService, kafkaService } from "../config/container";

const router = Router();
const sendMessage = createMessagingController(storageService, telegramService, kafkaService);

router.post("/send", sendMessage);

export default router;