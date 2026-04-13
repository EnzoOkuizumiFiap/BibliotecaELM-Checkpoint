using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibliotecaELM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PG_Authors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    NomeAutor = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Nascimento = table.Column<string>(type: "NVARCHAR2(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PG_Authors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PG_Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    NomeUsuario = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Nascimento = table.Column<string>(type: "NVARCHAR2(10)", nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Cpf = table.Column<string>(type: "NVARCHAR2(11)", maxLength: 11, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PG_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PG_Books",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    NomeLivro = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    Preco = table.Column<decimal>(type: "DECIMAL(10,2)", precision: 10, scale: 2, nullable: false),
                    DataLancamento = table.Column<string>(type: "NVARCHAR2(10)", nullable: false),
                    AutorId = table.Column<Guid>(type: "RAW(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PG_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PG_Books_PG_Authors_AutorId",
                        column: x => x.AutorId,
                        principalTable: "PG_Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Compras",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    FormaCompra = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DataCompra = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "RAW(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Compras_PG_Users_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "PG_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Emprestimos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    DataEmprestimo = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DataDevolucao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "RAW(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emprestimos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Emprestimos_PG_Users_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "PG_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PG_Addresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Cep = table.Column<string>(type: "NVARCHAR2(9)", maxLength: 9, nullable: false),
                    Estado = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    Cidade = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Bairro = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Rua = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    UsuarioId = table.Column<Guid>(type: "RAW(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PG_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PG_Addresses_PG_Users_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "PG_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompraLivro",
                columns: table => new
                {
                    ComprasId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    LivrosId = table.Column<Guid>(type: "RAW(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompraLivro", x => new { x.ComprasId, x.LivrosId });
                    table.ForeignKey(
                        name: "FK_CompraLivro_Compras_ComprasId",
                        column: x => x.ComprasId,
                        principalTable: "Compras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompraLivro_PG_Books_LivrosId",
                        column: x => x.LivrosId,
                        principalTable: "PG_Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmprestimoLivro",
                columns: table => new
                {
                    EmprestimosId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    LivrosId = table.Column<Guid>(type: "RAW(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmprestimoLivro", x => new { x.EmprestimosId, x.LivrosId });
                    table.ForeignKey(
                        name: "FK_EmprestimoLivro_Emprestimos_EmprestimosId",
                        column: x => x.EmprestimosId,
                        principalTable: "Emprestimos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmprestimoLivro_PG_Books_LivrosId",
                        column: x => x.LivrosId,
                        principalTable: "PG_Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompraLivro_LivrosId",
                table: "CompraLivro",
                column: "LivrosId");

            migrationBuilder.CreateIndex(
                name: "IX_Compras_UsuarioId",
                table: "Compras",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_EmprestimoLivro_LivrosId",
                table: "EmprestimoLivro",
                column: "LivrosId");

            migrationBuilder.CreateIndex(
                name: "IX_Emprestimos_UsuarioId",
                table: "Emprestimos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_PG_Addresses_UsuarioId",
                table: "PG_Addresses",
                column: "UsuarioId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PG_Books_AutorId",
                table: "PG_Books",
                column: "AutorId");

            migrationBuilder.CreateIndex(
                name: "IX_PG_Users_Email",
                table: "PG_Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompraLivro");

            migrationBuilder.DropTable(
                name: "EmprestimoLivro");

            migrationBuilder.DropTable(
                name: "PG_Addresses");

            migrationBuilder.DropTable(
                name: "Compras");

            migrationBuilder.DropTable(
                name: "Emprestimos");

            migrationBuilder.DropTable(
                name: "PG_Books");

            migrationBuilder.DropTable(
                name: "PG_Users");

            migrationBuilder.DropTable(
                name: "PG_Authors");
        }
    }
}
