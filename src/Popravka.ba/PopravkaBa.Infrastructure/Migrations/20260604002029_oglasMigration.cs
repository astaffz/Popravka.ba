using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PopravkaBa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class oglasMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrojPrijava",
                table: "OglasRadnoMjesto");

            migrationBuilder.CreateIndex(
                name: "IX_Oglas_DatumObjave",
                table: "Oglas",
                column: "DatumObjave");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Oglas_DatumObjave",
                table: "Oglas");

            migrationBuilder.AddColumn<int>(
                name: "BrojPrijava",
                table: "OglasRadnoMjesto",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
