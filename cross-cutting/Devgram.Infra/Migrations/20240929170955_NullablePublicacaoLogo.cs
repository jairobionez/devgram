using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Devgram.Infra.Migrations
{
    /// <inheritdoc />
    public partial class NullablePublicacaoLogo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Logo",
                table: "Publicacao",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Titulo",
                table: "Publicacao",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Logo",
                table: "Publicacao");

            migrationBuilder.DropColumn(
                name: "Titulo",
                table: "Publicacao");
        }
    }
}
