import { Request, Response } from "express";
import { SendMessageRequest } from "../interfaces/sendMessageRequest.interface";
import { SendMessageResponse } from "../interfaces/sendMessageResponse.interface";
import { storageService, telegramService, kafkaService } from "../config/container";
import { logInfo, logError } from "../utils/logger";
import { getErrorMessage } from "../utils/errorUtils";

export function createMessagingController(
    storage = storageService,
    telegram = telegramService,
    kafka = kafkaService
){
    return async function sendMessage(req: Request, res: Response) {
        const body: SendMessageRequest = req.body;
        logInfo("Nueva solicitud de envío recibida", { correlationId: body.correlationId, platform: body.platform });
        try {
            const fileBuffer = await storage.getFile(body.correlationId);
            if (!fileBuffer) {
                logError("Archivo no encontrado", { correlationId: body.correlationId });
                await kafka.log("error", { correlationId: body.correlationId, message: "Archivo no encontrado" });
                return res.status(404).json({ error: "Archivo no encontrado" });
            }
            if (body.platform === "telegram") {
                await telegram.sendFile(body.chatId, fileBuffer, body.message);
            }else {
                return res.status(400).json({ error: "Plataforma no soportada" });
            }

            const response: SendMessageResponse = {
                success: true,
                message: "Archivo enviado correctamente",
            };

            await kafka.log("success", {
                correlationId: body.correlationId,
                platform: body.platform,
                chatId: body.chatId,
                message: response.message,
            });

            logInfo("Archivo enviado correctamente", { correlationId: body.correlationId });
            res.status(200).json(response);
        } catch (error: unknown) {
            logError("Error al procesar envío", { error: getErrorMessage(error) });
            await kafka.log("error", { correlationId: body.correlationId, error: getErrorMessage(error) });
            res.status(500).json({ error: "Error interno del servidor" });
        }
    }
}