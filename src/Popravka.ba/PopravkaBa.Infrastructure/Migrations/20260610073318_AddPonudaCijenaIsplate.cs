using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PopravkaBa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPonudaCijenaIsplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Cijena",
                table: "PonudaUsluge",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Poruka",
                table: "PonudaUsluge",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TipIsplate",
                table: "PonudaUsluge",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cijena",
                table: "PonudaUsluge");

            migrationBuilder.DropColumn(
                name: "Poruka",
                table: "PonudaUsluge");

            migrationBuilder.DropColumn(
                name: "TipIsplate",
                table: "PonudaUsluge");
        }
    }
}
