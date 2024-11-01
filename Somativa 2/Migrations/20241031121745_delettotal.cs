using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Somativa_2.Migrations
{
    /// <inheritdoc />
    public partial class delettotal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pacientes_Planos_de_Saude_ConsultasPlanodeSaudeId",
                table: "Pacientes");

            migrationBuilder.RenameColumn(
                name: "ConsultasPlanodeSaudeId",
                table: "Pacientes",
                newName: "ConsultasConsultaId");

            migrationBuilder.RenameIndex(
                name: "IX_Pacientes_ConsultasPlanodeSaudeId",
                table: "Pacientes",
                newName: "IX_Pacientes_ConsultasConsultaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pacientes_Consultas_ConsultasConsultaId",
                table: "Pacientes",
                column: "ConsultasConsultaId",
                principalTable: "Consultas",
                principalColumn: "ConsultaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pacientes_Consultas_ConsultasConsultaId",
                table: "Pacientes");

            migrationBuilder.RenameColumn(
                name: "ConsultasConsultaId",
                table: "Pacientes",
                newName: "ConsultasPlanodeSaudeId");

            migrationBuilder.RenameIndex(
                name: "IX_Pacientes_ConsultasConsultaId",
                table: "Pacientes",
                newName: "IX_Pacientes_ConsultasPlanodeSaudeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pacientes_Planos_de_Saude_ConsultasPlanodeSaudeId",
                table: "Pacientes",
                column: "ConsultasPlanodeSaudeId",
                principalTable: "Planos_de_Saude",
                principalColumn: "PlanodeSaudeId");
        }
    }
}
