using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEBTimViec.Migrations
{
    /// <inheritdoc />
    public partial class SaveJob1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_saveJobs_AspNetUsers_UserId1",
                table: "saveJobs");

            migrationBuilder.DropIndex(
                name: "IX_saveJobs_UserId1",
                table: "saveJobs");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "saveJobs");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "saveJobs",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_saveJobs_UserId",
                table: "saveJobs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_saveJobs_AspNetUsers_UserId",
                table: "saveJobs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_saveJobs_AspNetUsers_UserId",
                table: "saveJobs");

            migrationBuilder.DropIndex(
                name: "IX_saveJobs_UserId",
                table: "saveJobs");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "saveJobs",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "saveJobs",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_saveJobs_UserId1",
                table: "saveJobs",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_saveJobs_AspNetUsers_UserId1",
                table: "saveJobs",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
