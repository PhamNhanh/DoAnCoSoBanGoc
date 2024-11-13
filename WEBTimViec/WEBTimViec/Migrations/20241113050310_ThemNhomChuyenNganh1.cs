using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEBTimViec.Migrations
{
    /// <inheritdoc />
    public partial class ThemNhomChuyenNganh1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "nhomChuyenNganhs",
                columns: table => new
                {
                    NhomChuyenNganhId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNhom = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nhomChuyenNganhs", x => x.NhomChuyenNganhId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
