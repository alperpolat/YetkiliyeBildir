using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YetkiliyeBildir.Migrations
{
    /// <inheritdoc />
    public partial class ReportFirmaAksiyonVeNot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirmaAksiyon",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirmaNotu",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirmaAksiyon",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "FirmaNotu",
                table: "Reports");
        }
    }
}
