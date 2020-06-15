using Microsoft.EntityFrameworkCore.Migrations;

namespace TranslationFramework.Dados.Migrations
{
    public partial class AlteraçãocolunalinhaArquivo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Coluna",
                table: "LinhasArquivo",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Coluna",
                table: "LinhasArquivo",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
