import axios from "axios";
import { IStorageService } from "../interfaces/storageService.interface";
import { getErrorMessage } from "../utils/errorUtils";

export class StorageService implements IStorageService {
  async getFile(correlationId: string): Promise<Buffer | null> {
    try {
      const response = await axios.get<ArrayBuffer>(
        `${process.env.STORAGE_SERVER_URL}/${correlationId}`,
        { responseType: "arraybuffer" }
      );

      return Buffer.from(response.data);
    } catch (error: unknown) {
      console.error("Error al obtener archivo del Storage:", getErrorMessage(error));
      return null;
    }
  }
}