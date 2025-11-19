using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KafkaWorkerService.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStorageLogSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StorageLogs_CorrelationId",
                table: "StorageLogs");

            migrationBuilder.DropColumn(
                name: "Endpoint",
                table: "StorageLogs");

            migrationBuilder.DropColumn(
                name: "Payload",
                table: "StorageLogs");

            migrationBuilder.DropColumn(
                name: "Service",
                table: "StorageLogs");

            migrationBuilder.DropColumn(
                name: "Success",
                table: "StorageLogs");

            migrationBuilder.AddColumn<string>(
                name: "Level",
                table: "StorageLogs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "StorageLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_StorageLogs_CorrelationId",
                table: "StorageLogs",
                column: "CorrelationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StorageLogs_CorrelationId",
                table: "StorageLogs");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "StorageLogs");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "StorageLogs");

            migrationBuilder.AddColumn<string>(
                name: "Endpoint",
                table: "StorageLogs",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Payload",
                table: "StorageLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Service",
                table: "StorageLogs",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Success",
                table: "StorageLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_StorageLogs_CorrelationId",
                table: "StorageLogs",
                column: "CorrelationId",
                unique: true);
        }
    }
}
