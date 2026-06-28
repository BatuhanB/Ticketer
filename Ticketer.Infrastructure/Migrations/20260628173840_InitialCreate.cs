using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ticketer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShowInventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ShowId = table.Column<Guid>(type: "uuid", nullable: false),
                    ShowName = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShowInventories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Seats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SeatNumber = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    IsAvailable = table.Column<bool>(type: "boolean", nullable: false),
                    ReservedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    ShowInventoryId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seats_ShowInventories_ShowInventoryId",
                        column: x => x.ShowInventoryId,
                        principalTable: "ShowInventories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Seats_ShowInventoryId",
                table: "Seats",
                column: "ShowInventoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Seats");

            migrationBuilder.DropTable(
                name: "ShowInventories");
        }
    }
}
