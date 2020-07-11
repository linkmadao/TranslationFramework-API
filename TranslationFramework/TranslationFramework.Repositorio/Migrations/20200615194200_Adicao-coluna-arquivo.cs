using Microsoft.EntityFrameworkCore.Migrations;

namespace TranslationFramework.Dados.Migrations
{
    public partial class Adicaocolunaarquivo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Coluna",
                table: "LinhasArquivo",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Coluna",
                table: "LinhasArquivo");
        }
    }
}
