using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KafkaWorkerService.Migrations
{
    /// <inheritdoc />
    public partial class AddHangfireLogsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HangfireLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CorrelationId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Service = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Endpoint = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Payload = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Success = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HangfireLogs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HangfireLogs_CorrelationId",
                table: "HangfireLogs",
                column: "CorrelationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HangfireLogs_CreatedAt",
                table: "HangfireLogs",
                column: "CreatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HangfireLogs");
        }
    }
}
