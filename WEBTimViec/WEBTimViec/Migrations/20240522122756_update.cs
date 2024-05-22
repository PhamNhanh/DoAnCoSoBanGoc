using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEBTimViec.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_baiTuyenDungs_kyNangMems_KyNangMemKNMem_id",
                table: "baiTuyenDungs");

            migrationBuilder.RenameColumn(
                name: "KNMem_id",
                table: "kyNangMems",
                newName: "KNMemid");

            migrationBuilder.RenameColumn(
                name: "KyNangMemKNMem_id",
                table: "baiTuyenDungs",
                newName: "KyNangMemKNMemid");

            migrationBuilder.RenameIndex(
                name: "IX_baiTuyenDungs_KyNangMemKNMem_id",
                table: "baiTuyenDungs",
                newName: "IX_baiTuyenDungs_KyNangMemKNMemid");

            migrationBuilder.AddForeignKey(
                name: "FK_baiTuyenDungs_kyNangMems_KyNangMemKNMemid",
                table: "baiTuyenDungs",
                column: "KyNangMemKNMemid",
                principalTable: "kyNangMems",
                principalColumn: "KNMemid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_baiTuyenDungs_kyNangMems_KyNangMemKNMemid",
                table: "baiTuyenDungs");

            migrationBuilder.RenameColumn(
                name: "KNMemid",
                table: "kyNangMems",
                newName: "KNMem_id");

            migrationBuilder.RenameColumn(
                name: "KyNangMemKNMemid",
                table: "baiTuyenDungs",
                newName: "KyNangMemKNMem_id");

            migrationBuilder.RenameIndex(
                name: "IX_baiTuyenDungs_KyNangMemKNMemid",
                table: "baiTuyenDungs",
                newName: "IX_baiTuyenDungs_KyNangMemKNMem_id");

            migrationBuilder.AddForeignKey(
                name: "FK_baiTuyenDungs_kyNangMems_KyNangMemKNMem_id",
                table: "baiTuyenDungs",
                column: "KyNangMemKNMem_id",
                principalTable: "kyNangMems",
                principalColumn: "KNMem_id");
        }
    }
}
