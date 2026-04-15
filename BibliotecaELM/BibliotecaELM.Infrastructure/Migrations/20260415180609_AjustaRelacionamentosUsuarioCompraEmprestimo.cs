using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibliotecaELM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AjustaRelacionamentosUsuarioCompraEmprestimo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BD_LoanBooks_Emprestimos_EmprestimosId",
                table: "BD_LoanBooks");

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

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataEmprestimo",
                table: "BD_Loans",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP(7)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataDevolucao",
                table: "BD_Loans",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP(7)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BD_Loans",
                table: "BD_Loans",
                column: "Id");

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
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BD_LoanBooks_BD_Loans_EmprestimosId",
                table: "BD_LoanBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_BD_Loans_BD_Users_UsuarioId",
                table: "BD_Loans");

            migrationBuilder.DropIndex(
                name: "IX_BD_Purchases_UsuarioId",
                table: "BD_Purchases");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BD_Loans",
                table: "BD_Loans");

            migrationBuilder.RenameTable(
                name: "BD_Loans",
                newName: "Emprestimos");

            migrationBuilder.RenameIndex(
                name: "IX_BD_Loans_UsuarioId",
                table: "Emprestimos",
                newName: "IX_Emprestimos_UsuarioId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataEmprestimo",
                table: "Emprestimos",
                type: "TIMESTAMP(7)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataDevolucao",
                table: "Emprestimos",
                type: "TIMESTAMP(7)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

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
                name: "FK_Emprestimos_BD_Users_UsuarioId",
                table: "Emprestimos",
                column: "UsuarioId",
                principalTable: "BD_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
