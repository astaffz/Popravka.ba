using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PopravkaBa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class VerificationMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VerifikacijskiToken",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KorisnikID = table.Column<string>(type: "text", nullable: false),
                    TokenHash = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Tip = table.Column<int>(type: "integer", nullable: false),
                    VrijemeGenerisanja = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VrijemeIsteka = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VrijemeKoristenja = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerifikacijskiToken", x => x.ID);
                    table.ForeignKey(
                        name: "FK_VerifikacijskiToken_AspNetUsers_KorisnikID",
                        column: x => x.KorisnikID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VerifikacijskiToken_KorisnikID_Tip_VrijemeGenerisanja",
                table: "VerifikacijskiToken",
                columns: new[] { "KorisnikID", "Tip", "VrijemeGenerisanja" });

            migrationBuilder.CreateIndex(
                name: "IX_VerifikacijskiToken_TokenHash",
                table: "VerifikacijskiToken",
                column: "TokenHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VerifikacijskiToken_VrijemeIsteka",
                table: "VerifikacijskiToken",
                column: "VrijemeIsteka");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VerifikacijskiToken");
        }
    }
}
