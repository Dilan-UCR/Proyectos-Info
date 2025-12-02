import TelegramBot from "node-telegram-bot-api";
import fs from "fs-extra";
import { ITelegramService } from "../interfaces/telegramService.interface";

export class TelegramService implements ITelegramService {
    private readonly bot: TelegramBot;

    constructor(token: string) {
        this.bot = new TelegramBot(token, { polling: false });
    }

    async sendFile(chatId: string, fileBuffer: Buffer, caption: string) {
        const tempPath = "./temp.pdf";
        await fs.writeFile(tempPath, fileBuffer);
        await this.bot.sendDocument(chatId, tempPath, { caption });
        await fs.unlink(tempPath);
    }
}