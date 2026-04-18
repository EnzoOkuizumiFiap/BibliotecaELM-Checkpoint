using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibliotecaELM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCp2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BD_Authors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    NomeAutor = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Nascimento = table.Column<string>(type: "NVARCHAR2(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BD_Authors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BD_Users",
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
                    table.PrimaryKey("PK_BD_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BD_Books",
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
                    table.PrimaryKey("PK_BD_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BD_Books_BD_Authors_AutorId",
                        column: x => x.AutorId,
                        principalTable: "BD_Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BD_Addresses",
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
                    table.PrimaryKey("PK_BD_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BD_Addresses_BD_Users_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "BD_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BD_Loans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    DataEmprestimo = table.Column<DateTime>(type: "date", nullable: false),
                    DataDevolucao = table.Column<DateTime>(type: "date", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "RAW(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BD_Loans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BD_Loans_BD_Users_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "BD_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BD_Purchases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    FormaCompra = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: false),
                    DataCompra = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "RAW(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BD_Purchases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BD_Purchases_BD_Users_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "BD_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BD_LoanBooks",
                columns: table => new
                {
                    EmprestimosId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    LivrosId = table.Column<Guid>(type: "RAW(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BD_LoanBooks", x => new { x.EmprestimosId, x.LivrosId });
                    table.ForeignKey(
                        name: "FK_BD_LoanBooks_BD_Books_LivrosId",
                        column: x => x.LivrosId,
                        principalTable: "BD_Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BD_LoanBooks_BD_Loans_EmprestimosId",
                        column: x => x.EmprestimosId,
                        principalTable: "BD_Loans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BD_PurchaseBooks",
                columns: table => new
                {
                    ComprasId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    LivrosId = table.Column<Guid>(type: "RAW(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BD_PurchaseBooks", x => new { x.ComprasId, x.LivrosId });
                    table.ForeignKey(
                        name: "FK_BD_PurchaseBooks_BD_Books_LivrosId",
                        column: x => x.LivrosId,
                        principalTable: "BD_Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BD_PurchaseBooks_BD_Purchases_ComprasId",
                        column: x => x.ComprasId,
                        principalTable: "BD_Purchases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BD_Addresses_UsuarioId",
                table: "BD_Addresses",
                column: "UsuarioId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BD_Books_AutorId",
                table: "BD_Books",
                column: "AutorId");

            migrationBuilder.CreateIndex(
                name: "IX_BD_LoanBooks_LivrosId",
                table: "BD_LoanBooks",
                column: "LivrosId");

            migrationBuilder.CreateIndex(
                name: "IX_BD_Loans_UsuarioId",
                table: "BD_Loans",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_BD_PurchaseBooks_LivrosId",
                table: "BD_PurchaseBooks",
                column: "LivrosId");

            migrationBuilder.CreateIndex(
                name: "IX_BD_Purchases_UsuarioId",
                table: "BD_Purchases",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BD_Addresses");

            migrationBuilder.DropTable(
                name: "BD_LoanBooks");

            migrationBuilder.DropTable(
                name: "BD_PurchaseBooks");

            migrationBuilder.DropTable(
                name: "BD_Loans");

            migrationBuilder.DropTable(
                name: "BD_Books");

            migrationBuilder.DropTable(
                name: "BD_Purchases");

            migrationBuilder.DropTable(
                name: "BD_Authors");

            migrationBuilder.DropTable(
                name: "BD_Users");
        }
    }
}
