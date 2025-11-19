using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KafkaWorkerService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Recipient = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsSuccessful = table.Column<bool>(type: "bit", nullable: false),
                    FailureReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HangfireLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    JobName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DurationSeconds = table.Column<double>(type: "float", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HangfireLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KafkaLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogType = table.Column<int>(type: "int", nullable: false),
                    Topic = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ErrorDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KafkaOffset = table.Column<long>(type: "bigint", nullable: false),
                    KafkaPartition = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KafkaLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MessageLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ReceiverId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MessageContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    MessageType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PdfLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileSizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    GeneratedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsSuccessful = table.Column<bool>(type: "bit", nullable: false),
                    GenerationError = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PdfLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StorageLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BucketName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ObjectKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OperationType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    OperationAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsSuccessful = table.Column<bool>(type: "bit", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageLogs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailLogs_CreatedAt",
                table: "EmailLogs",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_EmailLogs_Recipient",
                table: "EmailLogs",
                column: "Recipient");

            migrationBuilder.CreateIndex(
                name: "IX_HangfireLogs_CreatedAt",
                table: "HangfireLogs",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_HangfireLogs_JobId",
                table: "HangfireLogs",
                column: "JobId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KafkaLogs_CreatedAt",
                table: "KafkaLogs",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_KafkaLogs_Topic",
                table: "KafkaLogs",
                column: "Topic");

            migrationBuilder.CreateIndex(
                name: "IX_MessageLogs_CreatedAt",
                table: "MessageLogs",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_MessageLogs_ReceiverId",
                table: "MessageLogs",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageLogs_SenderId",
                table: "MessageLogs",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_PdfLogs_CreatedAt",
                table: "PdfLogs",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_PdfLogs_FileName",
                table: "PdfLogs",
                column: "FileName");

            migrationBuilder.CreateIndex(
                name: "IX_StorageLogs_BucketName",
                table: "StorageLogs",
                column: "BucketName");

            migrationBuilder.CreateIndex(
                name: "IX_StorageLogs_CreatedAt",
                table: "StorageLogs",
                column: "CreatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailLogs");

            migrationBuilder.DropTable(
                name: "HangfireLogs");

            migrationBuilder.DropTable(
                name: "KafkaLogs");

            migrationBuilder.DropTable(
                name: "MessageLogs");

            migrationBuilder.DropTable(
                name: "PdfLogs");

            migrationBuilder.DropTable(
                name: "StorageLogs");
        }
    }
}
