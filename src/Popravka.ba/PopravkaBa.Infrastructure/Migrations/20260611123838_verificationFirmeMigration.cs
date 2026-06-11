using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PopravkaBa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class verificationFirmeMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JIB",
                table: "VerifikacijaFirme",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PDVBroj",
                table: "VerifikacijaFirme",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JIB",
                table: "VerifikacijaFirme");

            migrationBuilder.DropColumn(
                name: "PDVBroj",
                table: "VerifikacijaFirme");
        }
    }
}
