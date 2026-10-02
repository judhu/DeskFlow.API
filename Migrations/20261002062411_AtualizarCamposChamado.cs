using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeskFlow.API.Migrations
{
    /// <inheritdoc />
    public partial class AtualizarCamposChamado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chamados_Categorias_CategoriaId",
                table: "Chamados");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataFechamento",
                table: "Chamados",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SolicitanteNome",
                table: "Chamados",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Solucao",
                table: "Chamados",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Interacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChamadoId = table.Column<int>(type: "int", nullable: false),
                    Autor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mensagem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Interacoes_Chamados_ChamadoId",
                        column: x => x.ChamadoId,
                        principalTable: "Chamados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Interacoes_ChamadoId",
                table: "Interacoes",
                column: "ChamadoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chamados_Categorias_CategoriaId",
                table: "Chamados",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chamados_Categorias_CategoriaId",
                table: "Chamados");

            migrationBuilder.DropTable(
                name: "Interacoes");

            migrationBuilder.DropColumn(
                name: "DataFechamento",
                table: "Chamados");

            migrationBuilder.DropColumn(
                name: "SolicitanteNome",
                table: "Chamados");

            migrationBuilder.DropColumn(
                name: "Solucao",
                table: "Chamados");

            migrationBuilder.AddForeignKey(
                name: "FK_Chamados_Categorias_CategoriaId",
                table: "Chamados",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
