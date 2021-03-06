﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TranslationFramework.Dados;

namespace TranslationFramework.Dados.Migrations
{
    [DbContext(typeof(AplicacaoContexto))]
    partial class AplicacaoContextoModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("TranslationFramework.Modelos.Arquivo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Caminho");

                    b.Property<DateTime>("DataUltimaAlteracao");

                    b.Property<string>("NomeArquivo");

                    b.Property<Guid>("ProjetoId");

                    b.HasKey("Id");

                    b.HasIndex("ProjetoId");

                    b.ToTable("Arquivos");
                });

            modelBuilder.Entity("TranslationFramework.Modelos.LinhaArquivo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("ArquivoId");

                    b.Property<string>("Coluna");

                    b.Property<int>("Linha");

                    b.Property<string>("Offset");

                    b.Property<byte[]>("Original");

                    b.Property<byte[]>("Traducao");

                    b.HasKey("Id");

                    b.HasIndex("ArquivoId");

                    b.ToTable("LinhasArquivo");
                });

            modelBuilder.Entity("TranslationFramework.Modelos.Projeto", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Nome");

                    b.Property<string>("SteamId");

                    b.HasKey("Id");

                    b.ToTable("Projetos");
                });

            modelBuilder.Entity("TranslationFramework.Modelos.Arquivo", b =>
                {
                    b.HasOne("TranslationFramework.Modelos.Projeto")
                        .WithMany("Arquivos")
                        .HasForeignKey("ProjetoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TranslationFramework.Modelos.LinhaArquivo", b =>
                {
                    b.HasOne("TranslationFramework.Modelos.Arquivo")
                        .WithMany("Linhas")
                        .HasForeignKey("ArquivoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
