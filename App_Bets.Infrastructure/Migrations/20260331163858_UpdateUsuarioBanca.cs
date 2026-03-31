using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App_Bets.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUsuarioBanca : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BancaPreferida",
                table: "Usuarios",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BancaPreferida",
                table: "Usuarios");
        }
    }
}
