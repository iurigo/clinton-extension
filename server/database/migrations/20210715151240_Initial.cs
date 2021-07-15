using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace server.database.migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "clinton-extension.employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Discipline = table.Column<int>(type: "int", nullable: false),
                    Rate = table.Column<float>(type: "real", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clinton-extension.employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "clinton-extension.system-settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clinton-extension.system-settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "clinton-extension.users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordSalt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clinton-extension.users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "clinton-extension.refresh-tokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clinton-extension.refresh-tokens", x => new { x.UserId, x.Token });
                    table.ForeignKey(
                        name: "FK_clinton-extension.refresh-tokens_clinton-extension.users_UserId",
                        column: x => x.UserId,
                        principalTable: "clinton-extension.users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "clinton-extension.system-logs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clinton-extension.system-logs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_clinton-extension.system-logs_clinton-extension.users_UserId",
                        column: x => x.UserId,
                        principalTable: "clinton-extension.users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "clinton-extension.system-logs",
                columns: new[] { "Id", "Date", "Details", "Source", "Type", "UserId" },
                values: new object[] { 1L, new DateTimeOffset(new DateTime(2021, 7, 15, 11, 12, 38, 885, DateTimeKind.Unspecified).AddTicks(3326), new TimeSpan(0, -4, 0, 0, 0)), null, "system", 0, null });

            migrationBuilder.InsertData(
                table: "clinton-extension.users",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "FullName", "PasswordHash", "PasswordSalt", "Role", "Status", "UpdatedAt", "Username" },
                values: new object[] { 1, new DateTimeOffset(new DateTime(2021, 7, 15, 11, 12, 38, 885, DateTimeKind.Unspecified).AddTicks(3326), new TimeSpan(0, -4, 0, 0, 0)), null, "Administrator", "$2a$11$Y2w46s/n.gv80rbudiU3zOhHQaz.iZphi1XQplEVo1vyS4py/9di6", "$2a$11$Y2w46s/n.gv80rbudiU3zO", 1, 1, new DateTimeOffset(new DateTime(2021, 7, 15, 11, 12, 38, 885, DateTimeKind.Unspecified).AddTicks(3326), new TimeSpan(0, -4, 0, 0, 0)), "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_clinton-extension.system-logs_UserId",
                table: "clinton-extension.system-logs",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "clinton-extension.employees");

            migrationBuilder.DropTable(
                name: "clinton-extension.refresh-tokens");

            migrationBuilder.DropTable(
                name: "clinton-extension.system-logs");

            migrationBuilder.DropTable(
                name: "clinton-extension.system-settings");

            migrationBuilder.DropTable(
                name: "clinton-extension.users");
        }
    }
}
