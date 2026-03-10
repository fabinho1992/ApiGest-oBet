using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App_Bets.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Coluna_CasaAposta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CasaAposta",
                table: "Bilhetes",
                type: "int",
                nullable: false,
                defaultValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CasaAposta",
                table: "Bilhetes");
        }
    }
}
