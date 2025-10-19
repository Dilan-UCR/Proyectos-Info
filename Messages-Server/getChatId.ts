import TelegramBot from "node-telegram-bot-api";
import dotenv from "dotenv";

dotenv.config();

const bot = new TelegramBot(process.env.TELEGRAM_BOT_TOKEN!, { polling: true });

console.log("Envía cualquier mensaje al bot para obtener tu chatId...");

bot.on("message", (msg) => {
  console.log("Usuario:", msg.from?.username);
  console.log("ChatId:", msg.chat.id);  
});