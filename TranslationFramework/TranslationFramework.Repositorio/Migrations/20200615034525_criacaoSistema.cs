using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TranslationFramework.Dados.Migrations
{
    public partial class criacaoSistema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projetos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Nome = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projetos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Arquivos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DataUltimaAlteracao = table.Column<DateTime>(nullable: false),
                    NomeArquivo = table.Column<string>(nullable: true),
                    Caminho = table.Column<string>(nullable: true),
                    PorcentagemTraduzida = table.Column<decimal>(nullable: false),
                    ProjetoId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Arquivos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Arquivos_Projeto_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "Projetos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LinhasArquivo",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Linha = table.Column<int>(nullable: false),
                    Offset = table.Column<string>(nullable: true),
                    Original = table.Column<string>(nullable: true),
                    Traducao = table.Column<string>(nullable: true),
                    ArquivoId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinhasArquivo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LinhasArquivo_Arquivos_ArquivoId",
                        column: x => x.ArquivoId,
                        principalTable: "Arquivos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Arquivos_ProjetoId",
                table: "Arquivos",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_LinhasArquivo_ArquivoId",
                table: "LinhasArquivo",
                column: "ArquivoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LinhasArquivo");

            migrationBuilder.DropTable(
                name: "Arquivos");

            migrationBuilder.DropTable(
                name: "Projeto");
        }
    }
}
