import dotenv from "dotenv";
dotenv.config();

import express from "express";
import messagingRoutes from "./routes/messagingRoutes";

const app = express();
app.use(express.json());
app.use("/api/messaging", messagingRoutes);

const PORT = process.env.PORT || 4000;
app.listen(PORT, () => console.log(`Messages Server corriendo en puerto ${PORT}`));
