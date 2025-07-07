using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YetkiliyeBildir.Migrations
{
    /// <inheritdoc />
    public partial class IdentityWithApplicationUserAndYetkiliKurum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdSoyad",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CheckStatus",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Telefon",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "YetkiliKurumId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "YetkiliKurumlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KurumAdi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KurumMail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KurumTelefon = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YetkiliKurumlar", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_YetkiliKurumId",
                table: "AspNetUsers",
                column: "YetkiliKurumId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_YetkiliKurumlar_YetkiliKurumId",
                table: "AspNetUsers",
                column: "YetkiliKurumId",
                principalTable: "YetkiliKurumlar",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_YetkiliKurumlar_YetkiliKurumId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "YetkiliKurumlar");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_YetkiliKurumId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AdSoyad",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CheckStatus",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Telefon",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "YetkiliKurumId",
                table: "AspNetUsers");
        }
    }
}
