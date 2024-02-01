﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SistemaDoLeoBlazor.API.Context;

#nullable disable

namespace SistemaDoLeoBlazor.API.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240131235921_atualizacao5")]
    partial class atualizacao5
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SistemaDoLeoBlazor.API.Entities.Categoria", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<bool>("inativo")
                        .HasColumnType("bit");

                    b.Property<string>("nome")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("id");

                    b.ToTable("Categoria");
                });

            modelBuilder.Entity("SistemaDoLeoBlazor.API.Entities.Cliente", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("bairro")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("cep")
                        .IsRequired()
                        .HasMaxLength(9)
                        .HasColumnType("nvarchar(9)");

                    b.Property<string>("cidade")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("complemento")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("documento")
                        .IsRequired()
                        .HasMaxLength(18)
                        .HasColumnType("nvarchar(18)");

                    b.Property<DateTime>("dtNasc")
                        .HasColumnType("date");

                    b.Property<string>("endereco")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("inativo")
                        .HasColumnType("bit");

                    b.Property<string>("nome")
                        .IsRequired()
                        .HasMaxLength(70)
                        .HasColumnType("nvarchar(70)");

                    b.Property<string>("numero")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.Property<bool>("tipoCliente")
                        .HasColumnType("bit");

                    b.Property<bool>("tipoForncedor")
                        .HasColumnType("bit");

                    b.Property<string>("uf")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("nvarchar(2)");

                    b.HasKey("id");

                    b.ToTable("Cliente");
                });

            modelBuilder.Entity("SistemaDoLeoBlazor.API.Entities.FormaPgto", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<bool>("inativo")
                        .HasColumnType("bit");

                    b.Property<string>("nome")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("id");

                    b.ToTable("FormaPgto");
                });

            modelBuilder.Entity("SistemaDoLeoBlazor.API.Entities.Operador", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<bool>("admin")
                        .HasColumnType("bit");

                    b.Property<bool>("inativo")
                        .HasColumnType("bit");

                    b.Property<string>("nome")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("senha")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("id");

                    b.ToTable("Operador");

                    b.HasData(
                        new
                        {
                            id = 1,
                            admin = true,
                            inativo = false,
                            nome = "Operador Padrão - Administrador",
                            senha = "5555"
                        });
                });

            modelBuilder.Entity("SistemaDoLeoBlazor.API.Entities.OperadorTela", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<bool>("ativo")
                        .HasColumnType("bit");

                    b.Property<bool>("editar")
                        .HasColumnType("bit");

                    b.Property<bool>("excluir")
                        .HasColumnType("bit");

                    b.Property<bool>("novo")
                        .HasColumnType("bit");

                    b.Property<int>("operadorId")
                        .HasColumnType("int");

                    b.Property<int>("telaId")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("operadorId");

                    b.HasIndex("telaId");

                    b.ToTable("OperadorTela");

                    b.HasData(
                        new
                        {
                            id = 1,
                            ativo = true,
                            editar = true,
                            excluir = true,
                            novo = true,
                            operadorId = 1,
                            telaId = 1
                        },
                        new
                        {
                            id = 2,
                            ativo = true,
                            editar = true,
                            excluir = true,
                            novo = true,
                            operadorId = 1,
                            telaId = 2
                        },
                        new
                        {
                            id = 3,
                            ativo = true,
                            editar = true,
                            excluir = true,
                            novo = true,
                            operadorId = 1,
                            telaId = 3
                        },
                        new
                        {
                            id = 4,
                            ativo = true,
                            editar = true,
                            excluir = true,
                            novo = true,
                            operadorId = 1,
                            telaId = 4
                        },
                        new
                        {
                            id = 5,
                            ativo = true,
                            editar = true,
                            excluir = true,
                            novo = true,
                            operadorId = 1,
                            telaId = 5
                        },
                        new
                        {
                            id = 6,
                            ativo = true,
                            editar = true,
                            excluir = true,
                            novo = true,
                            operadorId = 1,
                            telaId = 6
                        });
                });

            modelBuilder.Entity("SistemaDoLeoBlazor.API.Entities.Pedido", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<int>("clienteId")
                        .HasColumnType("int");

                    b.Property<DateTime>("data")
                        .HasColumnType("date");

                    b.Property<decimal>("desconto")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<int>("formaPgtoId")
                        .HasColumnType("int");

                    b.Property<string>("tipoOperacao")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("total")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<decimal>("valor")
                        .HasColumnType("decimal(10, 2)");

                    b.HasKey("id");

                    b.HasIndex("clienteId");

                    b.HasIndex("formaPgtoId");

                    b.ToTable("Pedido");
                });

            modelBuilder.Entity("SistemaDoLeoBlazor.API.Entities.PedidoItem", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<decimal>("desconto")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("pedidoId")
                        .HasColumnType("int");

                    b.Property<int>("produtoId")
                        .HasColumnType("int");

                    b.Property<int>("quantidade")
                        .HasColumnType("int");

                    b.Property<decimal>("total")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("valor")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("id");

                    b.HasIndex("pedidoId");

                    b.HasIndex("produtoId");

                    b.ToTable("PedidoItem");
                });

            modelBuilder.Entity("SistemaDoLeoBlazor.API.Entities.Produto", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<int>("categoriaId")
                        .HasColumnType("int");

                    b.Property<decimal>("custo")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<long>("estoque")
                        .HasColumnType("bigint");

                    b.Property<bool>("inativo")
                        .HasColumnType("bit");

                    b.Property<string>("nome")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<decimal>("preco")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<string>("unidade")
                        .IsRequired()
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.HasKey("id");

                    b.HasIndex("categoriaId");

                    b.ToTable("Produto");
                });

            modelBuilder.Entity("SistemaDoLeoBlazor.API.Entities.ProximoRegistro", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<int>("categoria")
                        .HasColumnType("int");

                    b.Property<int>("cliente")
                        .HasColumnType("int");

                    b.Property<int>("formaPgto")
                        .HasColumnType("int");

                    b.Property<int>("operador")
                        .HasColumnType("int");

                    b.Property<int>("pedido")
                        .HasColumnType("int");

                    b.Property<int>("produto")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.ToTable("ProximoRegistro");

                    b.HasData(
                        new
                        {
                            id = 1,
                            categoria = 0,
                            cliente = 0,
                            formaPgto = 0,
                            operador = 1,
                            pedido = 0,
                            produto = 0
                        });
                });

            modelBuilder.Entity("SistemaDoLeoBlazor.API.Entities.Tela", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("nome")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("id");

                    b.ToTable("Tela");

                    b.HasData(
                        new
                        {
                            id = 1,
                            nome = "Operador"
                        },
                        new
                        {
                            id = 2,
                            nome = "Categoria"
                        },
                        new
                        {
                            id = 3,
                            nome = "Cliente"
                        },
                        new
                        {
                            id = 4,
                            nome = "Forma Pagamento"
                        },
                        new
                        {
                            id = 5,
                            nome = "Pedido"
                        },
                        new
                        {
                            id = 6,
                            nome = "Produto"
                        });
                });

            modelBuilder.Entity("SistemaDoLeoBlazor.API.Entities.OperadorTela", b =>
                {
                    b.HasOne("SistemaDoLeoBlazor.API.Entities.Operador", "operador")
                        .WithMany()
                        .HasForeignKey("operadorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SistemaDoLeoBlazor.API.Entities.Tela", "tela")
                        .WithMany()
                        .HasForeignKey("telaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("operador");

                    b.Navigation("tela");
                });

            modelBuilder.Entity("SistemaDoLeoBlazor.API.Entities.Pedido", b =>
                {
                    b.HasOne("SistemaDoLeoBlazor.API.Entities.Cliente", "cliente")
                        .WithMany()
                        .HasForeignKey("clienteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SistemaDoLeoBlazor.API.Entities.FormaPgto", "formaPgto")
                        .WithMany()
                        .HasForeignKey("formaPgtoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("cliente");

                    b.Navigation("formaPgto");
                });

            modelBuilder.Entity("SistemaDoLeoBlazor.API.Entities.PedidoItem", b =>
                {
                    b.HasOne("SistemaDoLeoBlazor.API.Entities.Pedido", "pedido")
                        .WithMany()
                        .HasForeignKey("pedidoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SistemaDoLeoBlazor.API.Entities.Produto", "produto")
                        .WithMany()
                        .HasForeignKey("produtoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("pedido");

                    b.Navigation("produto");
                });

            modelBuilder.Entity("SistemaDoLeoBlazor.API.Entities.Produto", b =>
                {
                    b.HasOne("SistemaDoLeoBlazor.API.Entities.Categoria", "categoria")
                        .WithMany()
                        .HasForeignKey("categoriaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("categoria");
                });
#pragma warning restore 612, 618
        }
    }
}
