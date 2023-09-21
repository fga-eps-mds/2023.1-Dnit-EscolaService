using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace app.Migrations
{
    /// <inheritdoc />
    public partial class EtapaEnsino : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EtapaEnsino",
                table: "EtapaEnsino");

            migrationBuilder.RenameTable(
                name: "EtapaEnsino",
                newName: "EtapasEnsino");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EtapasEnsino",
                table: "EtapasEnsino",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EtapasEnsino",
                table: "EtapasEnsino");

            migrationBuilder.RenameTable(
                name: "EtapasEnsino",
                newName: "EtapaEnsino");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EtapaEnsino",
                table: "EtapaEnsino",
                column: "Id");
        }
    }
}
