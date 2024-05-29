using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEBTimViec.Migrations
{
    /// <inheritdoc />
    public partial class identity3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "applicationUserId",
                table: "hocVans",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "applicationUserId",
                table: "hocVans");
        }
    }
}
