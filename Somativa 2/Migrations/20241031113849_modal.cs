using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Somativa_2.Migrations
{
    /// <inheritdoc />
    public partial class modal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ConsultaId",
                table: "Pacientes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ConsultasPlanodeSaudeId",
                table: "Pacientes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pacientes_ConsultasPlanodeSaudeId",
                table: "Pacientes",
                column: "ConsultasPlanodeSaudeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pacientes_Planos_de_Saude_ConsultasPlanodeSaudeId",
                table: "Pacientes",
                column: "ConsultasPlanodeSaudeId",
                principalTable: "Planos_de_Saude",
                principalColumn: "PlanodeSaudeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pacientes_Planos_de_Saude_ConsultasPlanodeSaudeId",
                table: "Pacientes");

            migrationBuilder.DropIndex(
                name: "IX_Pacientes_ConsultasPlanodeSaudeId",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "ConsultaId",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "ConsultasPlanodeSaudeId",
                table: "Pacientes");
        }
    }
}
