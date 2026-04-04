using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App_Bets.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNameCasaPreferida : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BancaPreferida",
                table: "Usuarios",
                newName: "CasaPreferida");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CasaPreferida",
                table: "Usuarios",
                newName: "BancaPreferida");
        }
    }
}
