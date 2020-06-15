using Microsoft.EntityFrameworkCore.Migrations;

namespace TranslationFramework.Dados.Migrations
{
    public partial class AdicaoSteamId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PorcentagemTraduzida",
                table: "Arquivos");

            migrationBuilder.AddColumn<string>(
                name: "SteamId",
                table: "Projeto",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SteamId",
                table: "Projeto");

            migrationBuilder.AddColumn<decimal>(
                name: "PorcentagemTraduzida",
                table: "Arquivos",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
