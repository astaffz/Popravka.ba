using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PopravkaBa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class StatsUserNavigationMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
        "ALTER TABLE \"MjesecnaStatistikaKompozicija\" DROP COLUMN IF EXISTS \"IzvrsilacName\";");
           

            migrationBuilder.AddColumn<int>(
                name: "TipKorisnika",
                table: "MjesecnaStatistikaKompozicija",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "MjesecnaStatistikaKompozicija",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipKorisnika",
                table: "MjesecnaStatistikaKompozicija");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "MjesecnaStatistikaKompozicija");

            migrationBuilder.AddColumn<string>(
                name: "IzvrsilacName",
                table: "MjesecnaStatistikaKompozicija",
                type: "text",
                nullable: true);
        }
    }
}
