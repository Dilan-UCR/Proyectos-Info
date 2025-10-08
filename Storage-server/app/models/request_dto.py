from pydantic import BaseModel

class RequestDTO(BaseModel):
    correlationId: str
    clientId: int
    dateGeneration: str
