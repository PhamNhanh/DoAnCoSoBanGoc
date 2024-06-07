using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEBTimViec.Migrations
{
    /// <inheritdoc />
    public partial class xoahocvanchuyennganh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "hocvan_ChuyenNganhs");

            migrationBuilder.DropColumn(
                name: "KyNangMemId",
                table: "baiTuyenDungs");

            migrationBuilder.AddColumn<int>(
                name: "chuyenNganhId",
                table: "hocVans",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_hocVans_chuyenNganhId",
                table: "hocVans",
                column: "chuyenNganhId");

            migrationBuilder.AddForeignKey(
                name: "FK_hocVans_chuyenNganhs_chuyenNganhId",
                table: "hocVans",
                column: "chuyenNganhId",
                principalTable: "chuyenNganhs",
                principalColumn: "ChuyenNganh_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_hocVans_chuyenNganhs_chuyenNganhId",
                table: "hocVans");

            migrationBuilder.DropIndex(
                name: "IX_hocVans_chuyenNganhId",
                table: "hocVans");

            migrationBuilder.DropColumn(
                name: "chuyenNganhId",
                table: "hocVans");

            migrationBuilder.AddColumn<int>(
                name: "KyNangMemId",
                table: "baiTuyenDungs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "hocvan_ChuyenNganhs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChuyenNganhid = table.Column<int>(type: "int", nullable: true),
                    HocVanId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hocvan_ChuyenNganhs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_hocvan_ChuyenNganhs_chuyenNganhs_ChuyenNganhid",
                        column: x => x.ChuyenNganhid,
                        principalTable: "chuyenNganhs",
                        principalColumn: "ChuyenNganh_id");
                    table.ForeignKey(
                        name: "FK_hocvan_ChuyenNganhs_hocVans_HocVanId",
                        column: x => x.HocVanId,
                        principalTable: "hocVans",
                        principalColumn: "HocVan_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_hocvan_ChuyenNganhs_ChuyenNganhid",
                table: "hocvan_ChuyenNganhs",
                column: "ChuyenNganhid");

            migrationBuilder.CreateIndex(
                name: "IX_hocvan_ChuyenNganhs_HocVanId",
                table: "hocvan_ChuyenNganhs",
                column: "HocVanId");
        }
    }
}
