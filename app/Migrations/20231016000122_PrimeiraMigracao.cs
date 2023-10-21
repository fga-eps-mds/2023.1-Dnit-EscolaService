using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace app.Migrations
{
    /// <inheritdoc />
    public partial class PrimeiraMigracao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Municipios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Uf = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Municipios", x => x.Id);
                });

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
                    Observacao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Rede = table.Column<int>(type: "integer", nullable: false),
                    Uf = table.Column<int>(type: "integer", nullable: true),
                    Localizacao = table.Column<int>(type: "integer", nullable: true),
                    Porte = table.Column<int>(type: "integer", nullable: true),
                    Situacao = table.Column<int>(type: "integer", nullable: true),
                    MunicipioId = table.Column<int>(type: "integer", nullable: true),
                    DataAtualizacaoUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Escolas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Escolas_Municipios_MunicipioId",
                        column: x => x.MunicipioId,
                        principalTable: "Municipios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EscolaEtapaEnsino",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EscolaId = table.Column<Guid>(type: "uuid", nullable: false),
                    EtapaEnsino = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EscolaEtapaEnsino", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EscolaEtapaEnsino_Escolas_EscolaId",
                        column: x => x.EscolaId,
                        principalTable: "Escolas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EscolaEtapaEnsino_EscolaId",
                table: "EscolaEtapaEnsino",
                column: "EscolaId");

            migrationBuilder.CreateIndex(
                name: "IX_Escolas_MunicipioId",
                table: "Escolas",
                column: "MunicipioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EscolaEtapaEnsino");

            migrationBuilder.DropTable(
                name: "Escolas");

            migrationBuilder.DropTable(
                name: "Municipios");
        }
    }
}
