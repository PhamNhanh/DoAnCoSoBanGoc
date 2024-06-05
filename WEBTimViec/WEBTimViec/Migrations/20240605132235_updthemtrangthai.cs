using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEBTimViec.Migrations
{
    /// <inheritdoc />
    public partial class updthemtrangthai : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "TrangThai",
                table: "baiTuyenDungs",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrangThai",
                table: "baiTuyenDungs");
        }
    }
}
