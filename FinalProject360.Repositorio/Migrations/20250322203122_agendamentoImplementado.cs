using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject360.Repositorio.Migrations
{
    /// <inheritdoc />
    public partial class agendamentoImplementado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AgendamentoId",
                table: "Usuarios",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AgendamentoId",
                table: "Servicos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Agendamentos",
                columns: table => new
                {
                    AgendamentoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    ServicoId = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agendamentos", x => x.AgendamentoId);
                    table.ForeignKey(
                        name: "FK_Agendamentos_Servicos_ServicoId",
                        column: x => x.ServicoId,
                        principalTable: "Servicos",
                        principalColumn: "ServicoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Agendamentos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_AgendamentoId",
                table: "Usuarios",
                column: "AgendamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Servicos_AgendamentoId",
                table: "Servicos",
                column: "AgendamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Agendamentos_ServicoId",
                table: "Agendamentos",
                column: "ServicoId");

            migrationBuilder.CreateIndex(
                name: "IX_Agendamentos_UsuarioId",
                table: "Agendamentos",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Servicos_Agendamentos_AgendamentoId",
                table: "Servicos",
                column: "AgendamentoId",
                principalTable: "Agendamentos",
                principalColumn: "AgendamentoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Agendamentos_AgendamentoId",
                table: "Usuarios",
                column: "AgendamentoId",
                principalTable: "Agendamentos",
                principalColumn: "AgendamentoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Servicos_Agendamentos_AgendamentoId",
                table: "Servicos");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Agendamentos_AgendamentoId",
                table: "Usuarios");

            migrationBuilder.DropTable(
                name: "Agendamentos");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_AgendamentoId",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Servicos_AgendamentoId",
                table: "Servicos");

            migrationBuilder.DropColumn(
                name: "AgendamentoId",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "AgendamentoId",
                table: "Servicos");
        }
    }
}
