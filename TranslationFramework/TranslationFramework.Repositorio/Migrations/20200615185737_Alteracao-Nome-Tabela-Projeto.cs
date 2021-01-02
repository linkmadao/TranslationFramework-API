using Microsoft.EntityFrameworkCore.Migrations;

namespace TranslationFramework.Dados.Migrations
{
    public partial class AlteracaoNomeTabelaProjeto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Arquivos_Projeto_ProjetoId",
                table: "Arquivos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Projeto",
                table: "Projeto");

            migrationBuilder.RenameTable(
                name: "Projeto",
                newName: "Projetos");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Projetos",
                table: "Projetos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Arquivos_Projetos_ProjetoId",
                table: "Arquivos",
                column: "ProjetoId",
                principalTable: "Projetos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Arquivos_Projetos_ProjetoId",
                table: "Arquivos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Projetos",
                table: "Projetos");

            migrationBuilder.RenameTable(
                name: "Projetos",
                newName: "Projeto");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Projeto",
                table: "Projeto",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Arquivos_Projeto_ProjetoId",
                table: "Arquivos",
                column: "ProjetoId",
                principalTable: "Projeto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
