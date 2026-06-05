using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PopravkaBa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class oglasStatusMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusOglasa",
                table: "Oglas",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusOglasa",
                table: "Oglas");
        }
    }
}
