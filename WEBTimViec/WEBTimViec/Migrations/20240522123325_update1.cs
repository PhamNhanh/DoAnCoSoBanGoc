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
                name: "FK_baiTuyenDungs_kyNangMems_KyNangMemKNMemid",
                table: "baiTuyenDungs");

            migrationBuilder.DropColumn(
                name: "KNMemid",
                table: "baiTuyenDungs");

            migrationBuilder.RenameColumn(
                name: "KNMemid",
                table: "kyNangMems",
                newName: "KyNangMem_id");

            migrationBuilder.RenameColumn(
                name: "KyNangMemKNMemid",
                table: "baiTuyenDungs",
                newName: "KyNangMemId");

            migrationBuilder.RenameIndex(
                name: "IX_baiTuyenDungs_KyNangMemKNMemid",
                table: "baiTuyenDungs",
                newName: "IX_baiTuyenDungs_KyNangMemId");

            migrationBuilder.AddForeignKey(
                name: "FK_baiTuyenDungs_kyNangMems_KyNangMemId",
                table: "baiTuyenDungs",
                column: "KyNangMemId",
                principalTable: "kyNangMems",
                principalColumn: "KyNangMem_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_baiTuyenDungs_kyNangMems_KyNangMemId",
                table: "baiTuyenDungs");

            migrationBuilder.RenameColumn(
                name: "KyNangMem_id",
                table: "kyNangMems",
                newName: "KNMemid");

            migrationBuilder.RenameColumn(
                name: "KyNangMemId",
                table: "baiTuyenDungs",
                newName: "KyNangMemKNMemid");

            migrationBuilder.RenameIndex(
                name: "IX_baiTuyenDungs_KyNangMemId",
                table: "baiTuyenDungs",
                newName: "IX_baiTuyenDungs_KyNangMemKNMemid");

            migrationBuilder.AddColumn<int>(
                name: "KNMemid",
                table: "baiTuyenDungs",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_baiTuyenDungs_kyNangMems_KyNangMemKNMemid",
                table: "baiTuyenDungs",
                column: "KyNangMemKNMemid",
                principalTable: "kyNangMems",
                principalColumn: "KNMemid");
        }
    }
}
