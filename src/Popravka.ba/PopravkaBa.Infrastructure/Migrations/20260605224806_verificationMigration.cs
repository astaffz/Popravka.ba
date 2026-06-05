using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PopravkaBa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class verificationMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusVerifikacije",
                table: "AspNetUsers");

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
                name: "AdminVerificirao",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "StatusVerifikacije",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);
        }
    }
}
