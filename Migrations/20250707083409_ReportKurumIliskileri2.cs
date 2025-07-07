using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YetkiliyeBildir.Migrations
{
    /// <inheritdoc />
    public partial class ReportKurumIliskileri2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContactInfo",
                table: "YetkiliKurumlar",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "YetkiliKurumlar",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AuthorityId",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "YetkiliKurumId",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_AuthorityId",
                table: "Reports",
                column: "AuthorityId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_YetkiliKurumId",
                table: "Reports",
                column: "YetkiliKurumId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Authorities_AuthorityId",
                table: "Reports",
                column: "AuthorityId",
                principalTable: "Authorities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_YetkiliKurumlar_YetkiliKurumId",
                table: "Reports",
                column: "YetkiliKurumId",
                principalTable: "YetkiliKurumlar",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Authorities_AuthorityId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_YetkiliKurumlar_YetkiliKurumId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_AuthorityId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_YetkiliKurumId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ContactInfo",
                table: "YetkiliKurumlar");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "YetkiliKurumlar");

            migrationBuilder.DropColumn(
                name: "AuthorityId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "YetkiliKurumId",
                table: "Reports");
        }
    }
}
