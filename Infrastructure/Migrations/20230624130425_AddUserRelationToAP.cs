using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserRelationToAP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AccessPoints",
                keyColumn: "Id",
                keyValue: new Guid("20291a0d-58fc-400e-83fa-1d6fe5a47889"));

            migrationBuilder.DeleteData(
                table: "AccessPoints",
                keyColumn: "Id",
                keyValue: new Guid("e2baf6e6-452f-4890-a21a-988bd9887d75"));

            migrationBuilder.DeleteData(
                table: "AccessPoints",
                keyColumn: "Id",
                keyValue: new Guid("fe58ff46-66ae-406b-9677-96ac8dca9c8a"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("0e78642e-4c74-4d66-ac5c-c53d0d93a719"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("6734aeb5-8fdb-4b71-938f-f719defde846"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ebe628a5-b48a-4fcc-92d5-3c5ba4929e82"));

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Users",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<Guid>(
                name: "AccessPointModelId",
                table: "Users",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Roles",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AccessPoints",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Users",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Roles",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AccessPoints",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "AccessPoints",
                columns: new[] { "Id", "Name", "RequiredAccessLevel" },
                values: new object[,]
                {
                    { new Guid("20291a0d-58fc-400e-83fa-1d6fe5a47889"), "Store Room", 3 },
                    { new Guid("e2baf6e6-452f-4890-a21a-988bd9887d75"), "Directors Boardroom", 4 },
                    { new Guid("fe58ff46-66ae-406b-9677-96ac8dca9c8a"), "Entrance", 0 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessLevel", "EmailAddress", "FullName", "HasReportAccess", "PasswordHash", "PasswordSalt" },
                values: new object[,]
                {
                    { new Guid("0e78642e-4c74-4d66-ac5c-c53d0d93a719"), 2, "", "Alex", false, null, null },
                    { new Guid("6734aeb5-8fdb-4b71-938f-f719defde846"), 5, "", "Kevin (The Boss)", true, null, null },
                    { new Guid("ebe628a5-b48a-4fcc-92d5-3c5ba4929e82"), 3, "", "Jessica", true, null, null }
                });
        }
    }
}
