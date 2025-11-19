using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KafkaWorkerService.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEmailAndMessageLogsSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MessageLogs_CorrelationId",
                table: "MessageLogs");

            migrationBuilder.DropIndex(
                name: "IX_EmailLogs_CorrelationId",
                table: "EmailLogs");

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
                name: "Success",
                table: "MessageLogs");

            migrationBuilder.DropColumn(
                name: "Endpoint",
                table: "EmailLogs");

            migrationBuilder.DropColumn(
                name: "Payload",
                table: "EmailLogs");

            migrationBuilder.DropColumn(
                name: "Success",
                table: "EmailLogs");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "EmailLogs");

            migrationBuilder.RenameColumn(
                name: "Service",
                table: "EmailLogs",
                newName: "RecipientEmail");

            migrationBuilder.AddColumn<string>(
                name: "ChatId",
                table: "MessageLogs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "MessageLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Platform",
                table: "MessageLogs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "MessageLogs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "EmailLogs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Date",
                table: "EmailLogs",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Level",
                table: "EmailLogs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "EmailLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Time",
                table: "EmailLogs",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_MessageLogs_CorrelationId",
                table: "MessageLogs",
                column: "CorrelationId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailLogs_CorrelationId",
                table: "EmailLogs",
                column: "CorrelationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MessageLogs_CorrelationId",
                table: "MessageLogs");

            migrationBuilder.DropIndex(
                name: "IX_EmailLogs_CorrelationId",
                table: "EmailLogs");

            migrationBuilder.DropColumn(
                name: "ChatId",
                table: "MessageLogs");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "MessageLogs");

            migrationBuilder.DropColumn(
                name: "Platform",
                table: "MessageLogs");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "MessageLogs");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "EmailLogs");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "EmailLogs");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "EmailLogs");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "EmailLogs");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "EmailLogs");

            migrationBuilder.RenameColumn(
                name: "RecipientEmail",
                table: "EmailLogs",
                newName: "Service");

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

            migrationBuilder.AddColumn<bool>(
                name: "Success",
                table: "MessageLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Endpoint",
                table: "EmailLogs",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Payload",
                table: "EmailLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Success",
                table: "EmailLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "Timestamp",
                table: "EmailLogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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
    }
}
