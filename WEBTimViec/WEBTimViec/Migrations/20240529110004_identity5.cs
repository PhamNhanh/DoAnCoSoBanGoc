using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEBTimViec.Migrations
{
    /// <inheritdoc />
    public partial class identity5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_hocVans_HocVanid",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_HocVanid",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "HocVanid",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "applicationUserId",
                table: "hocVans",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_hocVans_applicationUserId",
                table: "hocVans",
                column: "applicationUserId",
                unique: true,
                filter: "[applicationUserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_hocVans_AspNetUsers_applicationUserId",
                table: "hocVans",
                column: "applicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_hocVans_AspNetUsers_applicationUserId",
                table: "hocVans");

            migrationBuilder.DropIndex(
                name: "IX_hocVans_applicationUserId",
                table: "hocVans");

            migrationBuilder.DropColumn(
                name: "applicationUserId",
                table: "hocVans");

            migrationBuilder.AddColumn<int>(
                name: "HocVanid",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_HocVanid",
                table: "AspNetUsers",
                column: "HocVanid",
                unique: true,
                filter: "[HocVanid] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_hocVans_HocVanid",
                table: "AspNetUsers",
                column: "HocVanid",
                principalTable: "hocVans",
                principalColumn: "HocVan_id");
        }
    }
}
