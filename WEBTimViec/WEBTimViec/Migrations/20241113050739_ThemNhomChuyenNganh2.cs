using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEBTimViec.Migrations
{
    /// <inheritdoc />
    public partial class ThemNhomChuyenNganh2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "nhomChuyenNganhid",
                table: "chuyenNganhs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_chuyenNganhs_nhomChuyenNganhid",
                table: "chuyenNganhs",
                column: "nhomChuyenNganhid");

            migrationBuilder.AddForeignKey(
                name: "FK_chuyenNganhs_nhomChuyenNganhs_nhomChuyenNganhid",
                table: "chuyenNganhs",
                column: "nhomChuyenNganhid",
                principalTable: "nhomChuyenNganhs",
                principalColumn: "NhomChuyenNganhId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_chuyenNganhs_nhomChuyenNganhs_nhomChuyenNganhid",
                table: "chuyenNganhs");

            migrationBuilder.DropIndex(
                name: "IX_chuyenNganhs_nhomChuyenNganhid",
                table: "chuyenNganhs");

            migrationBuilder.DropColumn(
                name: "nhomChuyenNganhid",
                table: "chuyenNganhs");
        }
    }
}
