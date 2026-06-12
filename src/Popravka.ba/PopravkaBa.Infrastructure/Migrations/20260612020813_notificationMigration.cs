using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PopravkaBa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class notificationMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OglasNaslov",
                table: "EmailNotifikacijaOglasa",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrimalacIme",
                table: "EmailNotifikacijaOglasa",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OglasNaslov",
                table: "EmailNotifikacijaOglasa");

            migrationBuilder.DropColumn(
                name: "PrimalacIme",
                table: "EmailNotifikacijaOglasa");
        }
    }
}
