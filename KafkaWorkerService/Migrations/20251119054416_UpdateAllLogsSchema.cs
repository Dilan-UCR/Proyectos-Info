using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KafkaWorkerService.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAllLogsSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StorageLogs_BucketName",
                table: "StorageLogs");

            migrationBuilder.DropIndex(
                name: "IX_PdfLogs_FileName",
                table: "PdfLogs");

            migrationBuilder.DropIndex(
                name: "IX_MessageLogs_ReceiverId",
                table: "MessageLogs");

            migrationBuilder.DropIndex(
                name: "IX_MessageLogs_SenderId",
                table: "MessageLogs");

            migrationBuilder.DropIndex(
                name: "IX_EmailLogs_Recipient",
                table: "EmailLogs");

            migrationBuilder.DropColumn(
                name: "ObjectKey",
                table: "StorageLogs");

            migrationBuilder.DropColumn(
                name: "OperationType",
                table: "StorageLogs");

            migrationBuilder.DropColumn(
                name: "SizeBytes",
                table: "StorageLogs");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "PdfLogs");

            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "PdfLogs");

            migrationBuilder.DropColumn(
                name: "FileSizeBytes",
                table: "PdfLogs");

            migrationBuilder.DropColumn(
                name: "MessageContent",
                table: "MessageLogs");

            migrationBuilder.DropColumn(
                name: "MessageType",
                table: "MessageLogs");

            migrationBuilder.DropColumn(
                name: "ReceiverId",
                table: "MessageLogs");

            migrationBuilder.DropColumn(
                name: "Body",
                table: "EmailLogs");

            migrationBuilder.DropColumn(
                name: "Subject",
                table: "EmailLogs");

            migrationBuilder.RenameColumn(
                name: "OperationAt",
                table: "StorageLogs",
                newName: "Timestamp");

            migrationBuilder.RenameColumn(
                name: "IsSuccessful",
                table: "StorageLogs",
                newName: "Success");

            migrationBuilder.RenameColumn(
                name: "ErrorMessage",
                table: "StorageLogs",
                newName: "Payload");

            migrationBuilder.RenameColumn(
                name: "BucketName",
                table: "StorageLogs",
                newName: "Service");

            migrationBuilder.RenameColumn(
                name: "IsSuccessful",
                table: "PdfLogs",
                newName: "Success");

            migrationBuilder.RenameColumn(
                name: "GenerationError",
                table: "PdfLogs",
                newName: "Payload");

            migrationBuilder.RenameColumn(
                name: "GeneratedAt",
                table: "PdfLogs",
                newName: "Timestamp");

            migrationBuilder.RenameColumn(
                name: "SentAt",
                table: "MessageLogs",
                newName: "Timestamp");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "MessageLogs",
                newName: "CorrelationId");

            migrationBuilder.RenameColumn(
                name: "IsRead",
                table: "MessageLogs",
                newName: "Success");

            migrationBuilder.RenameColumn(
                name: "SentAt",
                table: "EmailLogs",
                newName: "Timestamp");

            migrationBuilder.RenameColumn(
                name: "Recipient",
                table: "EmailLogs",
                newName: "Service");

            migrationBuilder.RenameColumn(
                name: "IsSuccessful",
                table: "EmailLogs",
                newName: "Success");

            migrationBuilder.RenameColumn(
                name: "FailureReason",
                table: "EmailLogs",
                newName: "Payload");

            migrationBuilder.AddColumn<string>(
                name: "CorrelationId",
                table: "StorageLogs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Endpoint",
                table: "StorageLogs",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CorrelationId",
                table: "PdfLogs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Endpoint",
                table: "PdfLogs",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Service",
                table: "PdfLogs",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Endpoint",
                table: "MessageLogs",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Payload",
                table: "MessageLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Service",
                table: "MessageLogs",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CorrelationId",
                table: "EmailLogs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Endpoint",
                table: "EmailLogs",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_StorageLogs_CorrelationId",
                table: "StorageLogs",
                column: "CorrelationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PdfLogs_CorrelationId",
                table: "PdfLogs",
                column: "CorrelationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MessageLogs_CorrelationId",
                table: "MessageLogs",
                column: "CorrelationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmailLogs_CorrelationId",
                table: "EmailLogs",
                column: "CorrelationId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StorageLogs_CorrelationId",
                table: "StorageLogs");

            migrationBuilder.DropIndex(
                name: "IX_PdfLogs_CorrelationId",
                table: "PdfLogs");

            migrationBuilder.DropIndex(
                name: "IX_MessageLogs_CorrelationId",
                table: "MessageLogs");

            migrationBuilder.DropIndex(
                name: "IX_EmailLogs_CorrelationId",
                table: "EmailLogs");

            migrationBuilder.DropColumn(
                name: "CorrelationId",
                table: "StorageLogs");

            migrationBuilder.DropColumn(
                name: "Endpoint",
                table: "StorageLogs");

            migrationBuilder.DropColumn(
                name: "CorrelationId",
                table: "PdfLogs");

            migrationBuilder.DropColumn(
                name: "Endpoint",
                table: "PdfLogs");

            migrationBuilder.DropColumn(
                name: "Service",
                table: "PdfLogs");

            migrationBuilder.DropColumn(
                name: "Endpoint",
                table: "MessageLogs");

            migrationBuilder.DropColumn(
                name: "Payload",
                table: "MessageLogs");

            migrationBuilder.DropColumn(
                name: "Service",
                table: "MessageLogs");

            migrationBuilder.DropColumn(
                name: "CorrelationId",
                table: "EmailLogs");

            migrationBuilder.DropColumn(
                name: "Endpoint",
                table: "EmailLogs");

            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "StorageLogs",
                newName: "OperationAt");

            migrationBuilder.RenameColumn(
                name: "Success",
                table: "StorageLogs",
                newName: "IsSuccessful");

            migrationBuilder.RenameColumn(
                name: "Service",
                table: "StorageLogs",
                newName: "BucketName");

            migrationBuilder.RenameColumn(
                name: "Payload",
                table: "StorageLogs",
                newName: "ErrorMessage");

            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "PdfLogs",
                newName: "GeneratedAt");

            migrationBuilder.RenameColumn(
                name: "Success",
                table: "PdfLogs",
                newName: "IsSuccessful");

            migrationBuilder.RenameColumn(
                name: "Payload",
                table: "PdfLogs",
                newName: "GenerationError");

            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "MessageLogs",
                newName: "SentAt");

            migrationBuilder.RenameColumn(
                name: "Success",
                table: "MessageLogs",
                newName: "IsRead");

            migrationBuilder.RenameColumn(
                name: "CorrelationId",
                table: "MessageLogs",
                newName: "SenderId");

            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "EmailLogs",
                newName: "SentAt");

            migrationBuilder.RenameColumn(
                name: "Success",
                table: "EmailLogs",
                newName: "IsSuccessful");

            migrationBuilder.RenameColumn(
                name: "Service",
                table: "EmailLogs",
                newName: "Recipient");

            migrationBuilder.RenameColumn(
                name: "Payload",
                table: "EmailLogs",
                newName: "FailureReason");

            migrationBuilder.AddColumn<string>(
                name: "ObjectKey",
                table: "StorageLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OperationType",
                table: "StorageLogs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "SizeBytes",
                table: "StorageLogs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "PdfLogs",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "PdfLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "FileSizeBytes",
                table: "PdfLogs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "MessageContent",
                table: "MessageLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MessageType",
                table: "MessageLogs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReceiverId",
                table: "MessageLogs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Body",
                table: "EmailLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "EmailLogs",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_StorageLogs_BucketName",
                table: "StorageLogs",
                column: "BucketName");

            migrationBuilder.CreateIndex(
                name: "IX_PdfLogs_FileName",
                table: "PdfLogs",
                column: "FileName");

            migrationBuilder.CreateIndex(
                name: "IX_MessageLogs_ReceiverId",
                table: "MessageLogs",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageLogs_SenderId",
                table: "MessageLogs",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailLogs_Recipient",
                table: "EmailLogs",
                column: "Recipient");
        }
    }
}
