using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace app.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaReferenciaASuperintendeciaMaisProximaNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SuperintedenenciaId",
                table: "Escolas",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuperintendenciaId",
                table: "Escolas",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Escolas_SuperintendenciaId",
                table: "Escolas",
                column: "SuperintendenciaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Escolas_Superintendencias_SuperintendenciaId",
                table: "Escolas",
                column: "SuperintendenciaId",
                principalTable: "Superintendencias",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Escolas_Superintendencias_SuperintendenciaId",
                table: "Escolas");

            migrationBuilder.DropIndex(
                name: "IX_Escolas_SuperintendenciaId",
                table: "Escolas");

            migrationBuilder.DropColumn(
                name: "SuperintedenenciaId",
                table: "Escolas");

            migrationBuilder.DropColumn(
                name: "SuperintendenciaId",
                table: "Escolas");
        }
    }
}
