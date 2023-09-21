using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace app.Migrations
{
    /// <inheritdoc />
    public partial class EscolaEtapaEnsino : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EscolaEtapaEnsino",
                columns: table => new
                {
                    EscolaId = table.Column<Guid>(type: "uuid", nullable: false),
                    EtapaEnsinoId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_EscolaEtapaEnsino_Escolas_EscolaId",
                        column: x => x.EscolaId,
                        principalTable: "Escolas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EscolaEtapaEnsino_EtapasEnsino_EtapaEnsinoId",
                        column: x => x.EtapaEnsinoId,
                        principalTable: "EtapasEnsino",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EscolaEtapaEnsino_EscolaId",
                table: "EscolaEtapaEnsino",
                column: "EscolaId");

            migrationBuilder.CreateIndex(
                name: "IX_EscolaEtapaEnsino_EtapaEnsinoId",
                table: "EscolaEtapaEnsino",
                column: "EtapaEnsinoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EscolaEtapaEnsino");
        }
    }
}
