using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEBTimViec.Migrations
{
    /// <inheritdoc />
    public partial class update1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_baiTuyenDungs_AspNetUsers_applicationUserId",
                table: "baiTuyenDungs");

            migrationBuilder.DropIndex(
                name: "IX_hocVans_applicationUserId",
                table: "hocVans");

            migrationBuilder.RenameColumn(
                name: "applicationUserId",
                table: "baiTuyenDungs",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_baiTuyenDungs_applicationUserId",
                table: "baiTuyenDungs",
                newName: "IX_baiTuyenDungs_ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_hocVans_applicationUserId",
                table: "hocVans",
                column: "applicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_baiTuyenDungs_AspNetUsers_ApplicationUserId",
                table: "baiTuyenDungs",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_baiTuyenDungs_AspNetUsers_ApplicationUserId",
                table: "baiTuyenDungs");

            migrationBuilder.DropIndex(
                name: "IX_hocVans_applicationUserId",
                table: "hocVans");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "baiTuyenDungs",
                newName: "applicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_baiTuyenDungs_ApplicationUserId",
                table: "baiTuyenDungs",
                newName: "IX_baiTuyenDungs_applicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_hocVans_applicationUserId",
                table: "hocVans",
                column: "applicationUserId",
                unique: true,
                filter: "[applicationUserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_baiTuyenDungs_AspNetUsers_applicationUserId",
                table: "baiTuyenDungs",
                column: "applicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
