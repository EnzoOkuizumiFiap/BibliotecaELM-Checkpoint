using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibliotecaELM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AchoQueVaiExplodir : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BD_LoanBooks_BD_Loans_EmprestimosId",
                table: "BD_LoanBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_BD_Loans_BD_Users_UsuarioId",
                table: "BD_Loans");

            migrationBuilder.DropForeignKey(
                name: "FK_BD_Purchases_BD_Users_UsuarioId",
                table: "BD_Purchases");

            migrationBuilder.DropIndex(
                name: "IX_BD_Users_Email",
                table: "BD_Users");

            migrationBuilder.DropIndex(
                name: "IX_BD_Purchases_UsuarioId",
                table: "BD_Purchases");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BD_Loans",
                table: "BD_Loans");

            migrationBuilder.DropColumn(
                name: "ValorTotal",
                table: "BD_Purchases");

            migrationBuilder.RenameTable(
                name: "BD_Loans",
                newName: "Emprestimos");

            migrationBuilder.RenameIndex(
                name: "IX_BD_Loans_UsuarioId",
                table: "Emprestimos",
                newName: "IX_Emprestimos_UsuarioId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Emprestimos",
                table: "Emprestimos",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_BD_Purchases_UsuarioId",
                table: "BD_Purchases",
                column: "UsuarioId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BD_LoanBooks_Emprestimos_EmprestimosId",
                table: "BD_LoanBooks",
                column: "EmprestimosId",
                principalTable: "Emprestimos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BD_Purchases_BD_Users_UsuarioId",
                table: "BD_Purchases",
                column: "UsuarioId",
                principalTable: "BD_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Emprestimos_BD_Users_UsuarioId",
                table: "Emprestimos",
                column: "UsuarioId",
                principalTable: "BD_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BD_LoanBooks_Emprestimos_EmprestimosId",
                table: "BD_LoanBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_BD_Purchases_BD_Users_UsuarioId",
                table: "BD_Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_Emprestimos_BD_Users_UsuarioId",
                table: "Emprestimos");

            migrationBuilder.DropIndex(
                name: "IX_BD_Purchases_UsuarioId",
                table: "BD_Purchases");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Emprestimos",
                table: "Emprestimos");

            migrationBuilder.RenameTable(
                name: "Emprestimos",
                newName: "BD_Loans");

            migrationBuilder.RenameIndex(
                name: "IX_Emprestimos_UsuarioId",
                table: "BD_Loans",
                newName: "IX_BD_Loans_UsuarioId");

            migrationBuilder.AddColumn<double>(
                name: "ValorTotal",
                table: "BD_Purchases",
                type: "BINARY_DOUBLE",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BD_Loans",
                table: "BD_Loans",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_BD_Users_Email",
                table: "BD_Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BD_Purchases_UsuarioId",
                table: "BD_Purchases",
                column: "UsuarioId");

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
                name: "FK_BD_Purchases_BD_Users_UsuarioId",
                table: "BD_Purchases",
                column: "UsuarioId",
                principalTable: "BD_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
