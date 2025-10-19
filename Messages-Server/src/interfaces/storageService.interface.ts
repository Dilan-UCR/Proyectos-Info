export interface IStorageService {
  getFile(correlationId: string): Promise<Buffer | null>;
}