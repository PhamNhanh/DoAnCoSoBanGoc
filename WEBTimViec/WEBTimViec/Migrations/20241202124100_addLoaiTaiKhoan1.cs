using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEBTimViec.Migrations
{
    /// <inheritdoc />
    public partial class addLoaiTaiKhoan1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "loaiTaiKhoanId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LoaiTaiKhoans",
                columns: table => new
                {
                    loaiTaiKhoanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tenLoaiTaiKhoan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    soNgayDung = table.Column<int>(type: "int", nullable: false),
                    gia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    soBaiTuyenDung = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiTaiKhoans", x => x.loaiTaiKhoanId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_loaiTaiKhoanId",
                table: "AspNetUsers",
                column: "loaiTaiKhoanId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_LoaiTaiKhoans_loaiTaiKhoanId",
                table: "AspNetUsers",
                column: "loaiTaiKhoanId",
                principalTable: "LoaiTaiKhoans",
                principalColumn: "loaiTaiKhoanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_LoaiTaiKhoans_loaiTaiKhoanId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "LoaiTaiKhoans");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_loaiTaiKhoanId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "loaiTaiKhoanId",
                table: "AspNetUsers");
        }
    }
}
