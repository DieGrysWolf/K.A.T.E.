using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAPsToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_AccessPoints_AccessPointModelId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_AccessPointModelId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AccessPointModelId",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "AccessPointModelUserModel",
                columns: table => new
                {
                    RegisteredAccessPointsId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RegisteredUsersId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessPointModelUserModel", x => new { x.RegisteredAccessPointsId, x.RegisteredUsersId });
                    table.ForeignKey(
                        name: "FK_AccessPointModelUserModel_AccessPoints_RegisteredAccessPoint~",
                        column: x => x.RegisteredAccessPointsId,
                        principalTable: "AccessPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccessPointModelUserModel_Users_RegisteredUsersId",
                        column: x => x.RegisteredUsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AccessPointModelUserModel_RegisteredUsersId",
                table: "AccessPointModelUserModel",
                column: "RegisteredUsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessPointModelUserModel");

            migrationBuilder.AddColumn<Guid>(
                name: "AccessPointModelId",
                table: "Users",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AccessPointModelId",
                table: "Users",
                column: "AccessPointModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_AccessPoints_AccessPointModelId",
                table: "Users",
                column: "AccessPointModelId",
                principalTable: "AccessPoints",
                principalColumn: "Id");
        }
    }
}
