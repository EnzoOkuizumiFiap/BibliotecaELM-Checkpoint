using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibliotecaELM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PadronizacaoPrefixoBD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompraLivro_Compras_ComprasId",
                table: "CompraLivro");

            migrationBuilder.DropForeignKey(
                name: "FK_CompraLivro_PG_Books_LivrosId",
                table: "CompraLivro");

            migrationBuilder.DropForeignKey(
                name: "FK_Compras_PG_Users_UsuarioId",
                table: "Compras");

            migrationBuilder.DropForeignKey(
                name: "FK_EmprestimoLivro_Emprestimos_EmprestimosId",
                table: "EmprestimoLivro");

            migrationBuilder.DropForeignKey(
                name: "FK_EmprestimoLivro_PG_Books_LivrosId",
                table: "EmprestimoLivro");

            migrationBuilder.DropForeignKey(
                name: "FK_Emprestimos_PG_Users_UsuarioId",
                table: "Emprestimos");

            migrationBuilder.DropForeignKey(
                name: "FK_PG_Addresses_PG_Users_UsuarioId",
                table: "PG_Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_PG_Books_PG_Authors_AutorId",
                table: "PG_Books");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PG_Users",
                table: "PG_Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PG_Books",
                table: "PG_Books");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PG_Authors",
                table: "PG_Authors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PG_Addresses",
                table: "PG_Addresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Emprestimos",
                table: "Emprestimos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmprestimoLivro",
                table: "EmprestimoLivro");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Compras",
                table: "Compras");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompraLivro",
                table: "CompraLivro");

            migrationBuilder.RenameTable(
                name: "PG_Users",
                newName: "BD_Users");

            migrationBuilder.RenameTable(
                name: "PG_Books",
                newName: "BD_Books");

            migrationBuilder.RenameTable(
                name: "PG_Authors",
                newName: "BD_Authors");

            migrationBuilder.RenameTable(
                name: "PG_Addresses",
                newName: "BD_Addresses");

            migrationBuilder.RenameTable(
                name: "Emprestimos",
                newName: "BD_Loans");

            migrationBuilder.RenameTable(
                name: "EmprestimoLivro",
                newName: "BD_LoanBooks");

            migrationBuilder.RenameTable(
                name: "Compras",
                newName: "BD_Purchases");

            migrationBuilder.RenameTable(
                name: "CompraLivro",
                newName: "BD_PurchaseBooks");

            migrationBuilder.RenameIndex(
                name: "IX_PG_Users_Email",
                table: "BD_Users",
                newName: "IX_BD_Users_Email");

            migrationBuilder.RenameIndex(
                name: "IX_PG_Books_AutorId",
                table: "BD_Books",
                newName: "IX_BD_Books_AutorId");

            migrationBuilder.RenameIndex(
                name: "IX_PG_Addresses_UsuarioId",
                table: "BD_Addresses",
                newName: "IX_BD_Addresses_UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_Emprestimos_UsuarioId",
                table: "BD_Loans",
                newName: "IX_BD_Loans_UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_EmprestimoLivro_LivrosId",
                table: "BD_LoanBooks",
                newName: "IX_BD_LoanBooks_LivrosId");

            migrationBuilder.RenameIndex(
                name: "IX_Compras_UsuarioId",
                table: "BD_Purchases",
                newName: "IX_BD_Purchases_UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_CompraLivro_LivrosId",
                table: "BD_PurchaseBooks",
                newName: "IX_BD_PurchaseBooks_LivrosId");

            migrationBuilder.AlterColumn<string>(
                name: "FormaCompra",
                table: "BD_Purchases",
                type: "NVARCHAR2(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "NUMBER(10)");

            migrationBuilder.AddColumn<double>(
                name: "ValorTotal",
                table: "BD_Purchases",
                type: "BINARY_DOUBLE",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BD_Users",
                table: "BD_Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BD_Books",
                table: "BD_Books",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BD_Authors",
                table: "BD_Authors",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BD_Addresses",
                table: "BD_Addresses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BD_Loans",
                table: "BD_Loans",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BD_LoanBooks",
                table: "BD_LoanBooks",
                columns: new[] { "EmprestimosId", "LivrosId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_BD_Purchases",
                table: "BD_Purchases",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BD_PurchaseBooks",
                table: "BD_PurchaseBooks",
                columns: new[] { "ComprasId", "LivrosId" });

            migrationBuilder.AddForeignKey(
                name: "FK_BD_Addresses_BD_Users_UsuarioId",
                table: "BD_Addresses",
                column: "UsuarioId",
                principalTable: "BD_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BD_Books_BD_Authors_AutorId",
                table: "BD_Books",
                column: "AutorId",
                principalTable: "BD_Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BD_LoanBooks_BD_Books_LivrosId",
                table: "BD_LoanBooks",
                column: "LivrosId",
                principalTable: "BD_Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BD_LoanBooks_BD_Loans_EmprestimosId",
                table: "BD_LoanBooks",
                column: "EmprestimosId",
                principalTable: "BD_Loans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BD_Loans_BD_Users_UsuarioId",
                table: "BD_Loans",
                column: "UsuarioId",
                principalTable: "BD_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BD_PurchaseBooks_BD_Books_LivrosId",
                table: "BD_PurchaseBooks",
                column: "LivrosId",
                principalTable: "BD_Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BD_PurchaseBooks_BD_Purchases_ComprasId",
                table: "BD_PurchaseBooks",
                column: "ComprasId",
                principalTable: "BD_Purchases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BD_Purchases_BD_Users_UsuarioId",
                table: "BD_Purchases",
                column: "UsuarioId",
                principalTable: "BD_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BD_Addresses_BD_Users_UsuarioId",
                table: "BD_Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_BD_Books_BD_Authors_AutorId",
                table: "BD_Books");

            migrationBuilder.DropForeignKey(
                name: "FK_BD_LoanBooks_BD_Books_LivrosId",
                table: "BD_LoanBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_BD_LoanBooks_BD_Loans_EmprestimosId",
                table: "BD_LoanBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_BD_Loans_BD_Users_UsuarioId",
                table: "BD_Loans");

            migrationBuilder.DropForeignKey(
                name: "FK_BD_PurchaseBooks_BD_Books_LivrosId",
                table: "BD_PurchaseBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_BD_PurchaseBooks_BD_Purchases_ComprasId",
                table: "BD_PurchaseBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_BD_Purchases_BD_Users_UsuarioId",
                table: "BD_Purchases");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BD_Users",
                table: "BD_Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BD_Purchases",
                table: "BD_Purchases");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BD_PurchaseBooks",
                table: "BD_PurchaseBooks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BD_Loans",
                table: "BD_Loans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BD_LoanBooks",
                table: "BD_LoanBooks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BD_Books",
                table: "BD_Books");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BD_Authors",
                table: "BD_Authors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BD_Addresses",
                table: "BD_Addresses");

            migrationBuilder.DropColumn(
                name: "ValorTotal",
                table: "BD_Purchases");

            migrationBuilder.RenameTable(
                name: "BD_Users",
                newName: "PG_Users");

            migrationBuilder.RenameTable(
                name: "BD_Purchases",
                newName: "Compras");

            migrationBuilder.RenameTable(
                name: "BD_PurchaseBooks",
                newName: "CompraLivro");

            migrationBuilder.RenameTable(
                name: "BD_Loans",
                newName: "Emprestimos");

            migrationBuilder.RenameTable(
                name: "BD_LoanBooks",
                newName: "EmprestimoLivro");

            migrationBuilder.RenameTable(
                name: "BD_Books",
                newName: "PG_Books");

            migrationBuilder.RenameTable(
                name: "BD_Authors",
                newName: "PG_Authors");

            migrationBuilder.RenameTable(
                name: "BD_Addresses",
                newName: "PG_Addresses");

            migrationBuilder.RenameIndex(
                name: "IX_BD_Users_Email",
                table: "PG_Users",
                newName: "IX_PG_Users_Email");

            migrationBuilder.RenameIndex(
                name: "IX_BD_Purchases_UsuarioId",
                table: "Compras",
                newName: "IX_Compras_UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_BD_PurchaseBooks_LivrosId",
                table: "CompraLivro",
                newName: "IX_CompraLivro_LivrosId");

            migrationBuilder.RenameIndex(
                name: "IX_BD_Loans_UsuarioId",
                table: "Emprestimos",
                newName: "IX_Emprestimos_UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_BD_LoanBooks_LivrosId",
                table: "EmprestimoLivro",
                newName: "IX_EmprestimoLivro_LivrosId");

            migrationBuilder.RenameIndex(
                name: "IX_BD_Books_AutorId",
                table: "PG_Books",
                newName: "IX_PG_Books_AutorId");

            migrationBuilder.RenameIndex(
                name: "IX_BD_Addresses_UsuarioId",
                table: "PG_Addresses",
                newName: "IX_PG_Addresses_UsuarioId");

            migrationBuilder.AlterColumn<int>(
                name: "FormaCompra",
                table: "Compras",
                type: "NUMBER(10)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(30)",
                oldMaxLength: 30);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PG_Users",
                table: "PG_Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Compras",
                table: "Compras",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompraLivro",
                table: "CompraLivro",
                columns: new[] { "ComprasId", "LivrosId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Emprestimos",
                table: "Emprestimos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmprestimoLivro",
                table: "EmprestimoLivro",
                columns: new[] { "EmprestimosId", "LivrosId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PG_Books",
                table: "PG_Books",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PG_Authors",
                table: "PG_Authors",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PG_Addresses",
                table: "PG_Addresses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompraLivro_Compras_ComprasId",
                table: "CompraLivro",
                column: "ComprasId",
                principalTable: "Compras",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompraLivro_PG_Books_LivrosId",
                table: "CompraLivro",
                column: "LivrosId",
                principalTable: "PG_Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Compras_PG_Users_UsuarioId",
                table: "Compras",
                column: "UsuarioId",
                principalTable: "PG_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmprestimoLivro_Emprestimos_EmprestimosId",
                table: "EmprestimoLivro",
                column: "EmprestimosId",
                principalTable: "Emprestimos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmprestimoLivro_PG_Books_LivrosId",
                table: "EmprestimoLivro",
                column: "LivrosId",
                principalTable: "PG_Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Emprestimos_PG_Users_UsuarioId",
                table: "Emprestimos",
                column: "UsuarioId",
                principalTable: "PG_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PG_Addresses_PG_Users_UsuarioId",
                table: "PG_Addresses",
                column: "UsuarioId",
                principalTable: "PG_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PG_Books_PG_Authors_AutorId",
                table: "PG_Books",
                column: "AutorId",
                principalTable: "PG_Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
