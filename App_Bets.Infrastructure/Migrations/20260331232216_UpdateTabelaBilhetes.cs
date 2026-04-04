using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App_Bets.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTabelaBilhetes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CasaAposta",
                table: "Bilhetes",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CasaAposta",
                table: "Bilhetes",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
