using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SistemaDoLeoBlazor.API.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categoria",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    inativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categoria", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    documento = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: false),
                    dtNasc = table.Column<DateTime>(type: "date", nullable: false),
                    inativo = table.Column<bool>(type: "bit", nullable: false),
                    cep = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    uf = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    cidade = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    bairro = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    endereco = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    numero = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    complemento = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    tipoCliente = table.Column<bool>(type: "bit", nullable: false),
                    tipoForncedor = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "FormaPgto",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    inativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormaPgto", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Operador",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    senha = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    admin = table.Column<bool>(type: "bit", nullable: false),
                    inativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operador", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ProximoRegistro",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    categoria = table.Column<int>(type: "int", nullable: false),
                    cliente = table.Column<int>(type: "int", nullable: false),
                    formaPgto = table.Column<int>(type: "int", nullable: false),
                    operador = table.Column<int>(type: "int", nullable: false),
                    pedido = table.Column<int>(type: "int", nullable: false),
                    produto = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProximoRegistro", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Tela",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tela", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Produto",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    categoriaId = table.Column<int>(type: "int", nullable: false),
                    nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    preco = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    custo = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    estoque = table.Column<long>(type: "bigint", nullable: false),
                    inativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produto", x => x.id);
                    table.ForeignKey(
                        name: "FK_Produto_Categoria_categoriaId",
                        column: x => x.categoriaId,
                        principalTable: "Categoria",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pedido",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    clienteId = table.Column<int>(type: "int", nullable: false),
                    formaPgtoId = table.Column<int>(type: "int", nullable: false),
                    data = table.Column<DateTime>(type: "date", nullable: false),
                    valor = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    desconto = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    total = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedido", x => x.id);
                    table.ForeignKey(
                        name: "FK_Pedido_Cliente_clienteId",
                        column: x => x.clienteId,
                        principalTable: "Cliente",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pedido_FormaPgto_formaPgtoId",
                        column: x => x.formaPgtoId,
                        principalTable: "FormaPgto",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OperadorTela",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    operadorId = table.Column<int>(type: "int", nullable: false),
                    telaId = table.Column<int>(type: "int", nullable: false),
                    ativo = table.Column<bool>(type: "bit", nullable: false),
                    novo = table.Column<bool>(type: "bit", nullable: false),
                    editar = table.Column<bool>(type: "bit", nullable: false),
                    excluir = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperadorTela", x => x.id);
                    table.ForeignKey(
                        name: "FK_OperadorTela_Operador_operadorId",
                        column: x => x.operadorId,
                        principalTable: "Operador",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OperadorTela_Tela_telaId",
                        column: x => x.telaId,
                        principalTable: "Tela",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PedidoItem",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    pedidoId = table.Column<int>(type: "int", nullable: false),
                    produtoId = table.Column<int>(type: "int", nullable: false),
                    valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    quantidade = table.Column<int>(type: "int", nullable: false),
                    desconto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    total = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoItem", x => x.id);
                    table.ForeignKey(
                        name: "FK_PedidoItem_Pedido_pedidoId",
                        column: x => x.pedidoId,
                        principalTable: "Pedido",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PedidoItem_Produto_produtoId",
                        column: x => x.produtoId,
                        principalTable: "Produto",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Operador",
                columns: new[] { "id", "admin", "inativo", "nome", "senha" },
                values: new object[] { 1, true, false, "Operador Padrão - Administrador", "5555" });

            migrationBuilder.InsertData(
                table: "ProximoRegistro",
                columns: new[] { "id", "categoria", "cliente", "formaPgto", "operador", "pedido", "produto" },
                values: new object[] { 1, 0, 0, 0, 0, 0, 0 });

            migrationBuilder.InsertData(
                table: "Tela",
                columns: new[] { "id", "nome" },
                values: new object[,]
                {
                    { 1, "Operador" },
                    { 2, "Categoria" },
                    { 3, "Cliente" },
                    { 4, "Forma Pagamento" },
                    { 5, "Pedido" },
                    { 6, "Produto" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OperadorTela_operadorId",
                table: "OperadorTela",
                column: "operadorId");

            migrationBuilder.CreateIndex(
                name: "IX_OperadorTela_telaId",
                table: "OperadorTela",
                column: "telaId");

            migrationBuilder.CreateIndex(
                name: "IX_Pedido_clienteId",
                table: "Pedido",
                column: "clienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Pedido_formaPgtoId",
                table: "Pedido",
                column: "formaPgtoId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoItem_pedidoId",
                table: "PedidoItem",
                column: "pedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoItem_produtoId",
                table: "PedidoItem",
                column: "produtoId");

            migrationBuilder.CreateIndex(
                name: "IX_Produto_categoriaId",
                table: "Produto",
                column: "categoriaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OperadorTela");

            migrationBuilder.DropTable(
                name: "PedidoItem");

            migrationBuilder.DropTable(
                name: "ProximoRegistro");

            migrationBuilder.DropTable(
                name: "Operador");

            migrationBuilder.DropTable(
                name: "Tela");

            migrationBuilder.DropTable(
                name: "Pedido");

            migrationBuilder.DropTable(
                name: "Produto");

            migrationBuilder.DropTable(
                name: "Cliente");

            migrationBuilder.DropTable(
                name: "FormaPgto");

            migrationBuilder.DropTable(
                name: "Categoria");
        }
    }
}
