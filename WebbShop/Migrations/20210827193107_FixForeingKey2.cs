using Microsoft.EntityFrameworkCore.Migrations;

namespace WebbShop.Migrations
{
    public partial class FixForeingKey2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_role_user_UserId",
                table: "role");

            migrationBuilder.DropIndex(
                name: "IX_role_UserId",
                table: "role");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "role");

            migrationBuilder.CreateTable(
                name: "RolesUser",
                columns: table => new
                {
                    roleId = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolesUser", x => new { x.roleId, x.userId });
                    table.ForeignKey(
                        name: "FK_RolesUser_role_roleId",
                        column: x => x.roleId,
                        principalTable: "role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolesUser_user_userId",
                        column: x => x.userId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RolesUser_userId",
                table: "RolesUser",
                column: "userId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RolesUser");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "role",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_role_UserId",
                table: "role",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_role_user_UserId",
                table: "role",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
