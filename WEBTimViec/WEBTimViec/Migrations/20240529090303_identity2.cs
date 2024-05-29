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
                name: "FK_hocvan_ChuyenNganhs_AspNetUsers_ApplicationUserId",
                table: "hocvan_ChuyenNganhs");

            migrationBuilder.DropIndex(
                name: "IX_hocvan_ChuyenNganhs_ApplicationUserId",
                table: "hocvan_ChuyenNganhs");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "hocvan_ChuyenNganhs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "hocvan_ChuyenNganhs",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_hocvan_ChuyenNganhs_ApplicationUserId",
                table: "hocvan_ChuyenNganhs",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_hocvan_ChuyenNganhs_AspNetUsers_ApplicationUserId",
                table: "hocvan_ChuyenNganhs",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
