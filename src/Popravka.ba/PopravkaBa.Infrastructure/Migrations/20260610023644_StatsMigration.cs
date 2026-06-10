using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PopravkaBa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class StatsMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DatumIzvrsavanjaUsluge",
                table: "PonudaUsluge",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MjesecnaStatistikaKompozicija",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Godina = table.Column<int>(type: "integer", nullable: false),
                    Mjesec = table.Column<int>(type: "integer", nullable: false),
                    IzvrsilacID = table.Column<string>(type: "text", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    Slika = table.Column<string>(type: "text", nullable: true),
                    KategorijaID = table.Column<int>(type: "integer", nullable: true),
                    KategorijaNaziv = table.Column<string>(type: "text", nullable: true),
                    MjestoID = table.Column<int>(type: "integer", nullable: true),
                    MjestoNaziv = table.Column<string>(type: "text", nullable: true),
                    ProsjecnaOcjena = table.Column<decimal>(type: "numeric", nullable: false),
                    BrojPoslova = table.Column<int>(type: "integer", nullable: false),
                    RangStandardni = table.Column<int>(type: "integer", nullable: false),
                    VrijemeAzuriranja = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IzvrsilacName = table.Column<String>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MjesecnaStatistikaKompozicija", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MjesecnaStatistikaKompozicija");

            migrationBuilder.DropColumn(
                name: "DatumIzvrsavanjaUsluge",
                table: "PonudaUsluge");
        }
    }
}
