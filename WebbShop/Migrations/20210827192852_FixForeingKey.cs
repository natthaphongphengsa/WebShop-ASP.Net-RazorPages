using Microsoft.EntityFrameworkCore.Migrations;

namespace WebbShop.Migrations
{
    public partial class FixForeingKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_role_RolesId",
                table: "user");

            migrationBuilder.DropIndex(
                name: "IX_user_RolesId",
                table: "user");

            migrationBuilder.DropColumn(
                name: "RolesId",
                table: "user");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "RolesId",
                table: "user",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_RolesId",
                table: "user",
                column: "RolesId");

            migrationBuilder.AddForeignKey(
                name: "FK_user_role_RolesId",
                table: "user",
                column: "RolesId",
                principalTable: "role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
