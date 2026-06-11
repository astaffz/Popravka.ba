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

            migrationBuilder.AlterColumn<int>(
               name: "StatusVerifikacije",
               table: "AspNetUsers",
               type: "integer",
               nullable: false,
               defaultValue: 0,
               oldClrType: typeof(int),
               oldType: "integer",
               oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AdminVerificirao",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false
                );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinIskustvo",
                table: "OglasRadnoMjesto");
            
            migrationBuilder.DropColumn(
                name: "AdminVerificirao",
                table: "AspNetUsers"
                );
            
            migrationBuilder.AlterColumn<int>(
                name: "StatusVerifikacije",
                table: "AspNetUsers",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: false,
                oldDefaultValue: 0);

        }
    }
}