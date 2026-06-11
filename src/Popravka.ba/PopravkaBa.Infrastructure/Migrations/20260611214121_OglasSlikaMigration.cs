using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PopravkaBa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OglasSlikaMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Slika",
                table: "OglasRadnoMjesto");

            migrationBuilder.AddColumn<string>(
                name: "Slika",
                table: "Oglas",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Slika",
                table: "Oglas");

            migrationBuilder.AddColumn<string>(
                name: "Slika",
                table: "OglasRadnoMjesto",
                type: "text",
                nullable: true);
        }
    }
}
