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
        
        logInfo("Nueva solicitud de envío recibida", { correlationId: body.CorrelationId, platform: body.Platform });
        
      
        if (!body.CorrelationId) {
            console.log('correlationId está vacío');
            logError("correlationId es requerido", { receivedData: req.body });
            return res.status(400).json({ 
                error: 'correlationId es requerido',
                receivedData: req.body 
            });
        }
        try {
            const fileBuffer = await storage.getFile(body.CorrelationId);
            
            if (!fileBuffer) {
                await kafka.log("error", { correlationId: body.CorrelationId, message: "Archivo no encontrado" });
                return res.status(404).json({ error: "Archivo no encontrado" });
            }
            
            if (body.Platform === "telegram") {
                await telegram.sendFile(body.ChatId, fileBuffer, body.Message);
            } else {
                return res.status(400).json({ error: "Plataforma no soportada" });
            }

            const response: SendMessageResponse = {
                success: true,
                message: "Archivo enviado correctamente",
            };

            await kafka.log("success", {
                correlationId: body.CorrelationId,
                platform: body.Platform,
                chatId: body.ChatId,
                message: response.message,
            });
            res.status(200).json(response);
        } catch (error: unknown) {
            logError("Error al procesar envío", { error: getErrorMessage(error), correlationId: body.CorrelationId });
            await kafka.log("error", { correlationId: body.CorrelationId, error: getErrorMessage(error) });
            res.status(500).json({ error: "Error interno del servidor" });
        }
    }
}