using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace app.Migrations
{
    /// <inheritdoc />
    public partial class Escola : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Escolas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Codigo = table.Column<int>(type: "integer", nullable: false),
                    Cep = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    Endereco = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Latitude = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    Longitude = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    TotalAlunos = table.Column<int>(type: "integer", nullable: false),
                    TotalDocentes = table.Column<int>(type: "integer", nullable: false),
                    Telefone = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    Observacao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    RedeId = table.Column<int>(type: "integer", nullable: false),
                    UfId = table.Column<short>(type: "smallint", nullable: false),
                    LocalizacaoId = table.Column<int>(type: "integer", nullable: false),
                    MunicipioId = table.Column<int>(type: "integer", nullable: false),
                    PorteId = table.Column<int>(type: "integer", nullable: false),
                    SituacaoId = table.Column<int>(type: "integer", nullable: false),
                    AtaualizacaoDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Escolas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Escolas_Localizacoes_LocalizacaoId",
                        column: x => x.LocalizacaoId,
                        principalTable: "Localizacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Escolas_Municipios_MunicipioId",
                        column: x => x.MunicipioId,
                        principalTable: "Municipios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Escolas_Portes_PorteId",
                        column: x => x.PorteId,
                        principalTable: "Portes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Escolas_Redes_RedeId",
                        column: x => x.RedeId,
                        principalTable: "Redes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Escolas_Situacao_SituacaoId",
                        column: x => x.SituacaoId,
                        principalTable: "Situacao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Escolas_UnidadesFederativas_UfId",
                        column: x => x.UfId,
                        principalTable: "UnidadesFederativas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Escolas_LocalizacaoId",
                table: "Escolas",
                column: "LocalizacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Escolas_MunicipioId",
                table: "Escolas",
                column: "MunicipioId");

            migrationBuilder.CreateIndex(
                name: "IX_Escolas_PorteId",
                table: "Escolas",
                column: "PorteId");

            migrationBuilder.CreateIndex(
                name: "IX_Escolas_RedeId",
                table: "Escolas",
                column: "RedeId");

            migrationBuilder.CreateIndex(
                name: "IX_Escolas_SituacaoId",
                table: "Escolas",
                column: "SituacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Escolas_UfId",
                table: "Escolas",
                column: "UfId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Escolas");
        }
    }
}
