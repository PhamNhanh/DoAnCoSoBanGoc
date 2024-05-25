using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEBTimViec.Migrations
{
    /// <inheritdoc />
    public partial class identity2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_baiTuyenDungs_AspNetUsers_applicationUserId1",
                table: "baiTuyenDungs");

            migrationBuilder.RenameColumn(
                name: "applicationUserId1",
                table: "baiTuyenDungs",
                newName: "applicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_baiTuyenDungs_applicationUserId1",
                table: "baiTuyenDungs",
                newName: "IX_baiTuyenDungs_applicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_baiTuyenDungs_AspNetUsers_applicationUserId",
                table: "baiTuyenDungs",
                column: "applicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_baiTuyenDungs_AspNetUsers_applicationUserId",
                table: "baiTuyenDungs");

            migrationBuilder.RenameColumn(
                name: "applicationUserId",
                table: "baiTuyenDungs",
                newName: "applicationUserId1");

            migrationBuilder.RenameIndex(
                name: "IX_baiTuyenDungs_applicationUserId",
                table: "baiTuyenDungs",
                newName: "IX_baiTuyenDungs_applicationUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_baiTuyenDungs_AspNetUsers_applicationUserId1",
                table: "baiTuyenDungs",
                column: "applicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
