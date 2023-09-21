using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace app.Migrations
{
    /// <inheritdoc />
    public partial class Situacoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Escolas_Situacao_SituacaoId",
                table: "Escolas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Situacao",
                table: "Situacao");

            migrationBuilder.RenameTable(
                name: "Situacao",
                newName: "Situacoes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Situacoes",
                table: "Situacoes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Escolas_Situacoes_SituacaoId",
                table: "Escolas",
                column: "SituacaoId",
                principalTable: "Situacoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Escolas_Situacoes_SituacaoId",
                table: "Escolas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Situacoes",
                table: "Situacoes");

            migrationBuilder.RenameTable(
                name: "Situacoes",
                newName: "Situacao");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Situacao",
                table: "Situacao",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Escolas_Situacao_SituacaoId",
                table: "Escolas",
                column: "SituacaoId",
                principalTable: "Situacao",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
