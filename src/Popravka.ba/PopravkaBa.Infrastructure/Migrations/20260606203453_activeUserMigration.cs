using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PopravkaBa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class activeUserMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MinIskustvo",
                table: "OglasRadnoMjesto",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StatusVerifikacije",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0
             );

            migrationBuilder.AddColumn<bool>(
                name: "AdminVerificirao",
                table: "AspNetUsers",
                type: "boolean",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinIskustvo",
                table: "OglasRadnoMjesto");

            migrationBuilder.DropColumn(
                name: "AdminVerificirao",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StatusVerifikacije",
                table: "AspNetUsers"
                );
        }
    }
}
