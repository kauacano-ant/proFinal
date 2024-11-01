using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Somativa_2.Migrations
{
    /// <inheritdoc />
    public partial class cascata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_Pacientes_PacienteId",
                table: "Consultas");

            migrationBuilder.DropForeignKey(
                name: "FK_Pacientes_Consultas_ConsultasConsultaId",
                table: "Pacientes");

            migrationBuilder.DropForeignKey(
                name: "FK_Planos_de_Saude_Consultas_ConsultasConsultaId",
                table: "Planos_de_Saude");

            migrationBuilder.DropIndex(
                name: "IX_Planos_de_Saude_ConsultasConsultaId",
                table: "Planos_de_Saude");

            migrationBuilder.DropIndex(
                name: "IX_Pacientes_ConsultasConsultaId",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "ConsultaId",
                table: "Planos_de_Saude");

            migrationBuilder.DropColumn(
                name: "ConsultasConsultaId",
                table: "Planos_de_Saude");

            migrationBuilder.DropColumn(
                name: "ConsultaId",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "ConsultasConsultaId",
                table: "Pacientes");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_Pacientes_PacienteId",
                table: "Consultas",
                column: "PacienteId",
                principalTable: "Pacientes",
                principalColumn: "PacienteId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_Pacientes_PacienteId",
                table: "Consultas");

            migrationBuilder.AddColumn<Guid>(
                name: "ConsultaId",
                table: "Planos_de_Saude",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ConsultasConsultaId",
                table: "Planos_de_Saude",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ConsultaId",
                table: "Pacientes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ConsultasConsultaId",
                table: "Pacientes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Planos_de_Saude_ConsultasConsultaId",
                table: "Planos_de_Saude",
                column: "ConsultasConsultaId");

            migrationBuilder.CreateIndex(
                name: "IX_Pacientes_ConsultasConsultaId",
                table: "Pacientes",
                column: "ConsultasConsultaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_Pacientes_PacienteId",
                table: "Consultas",
                column: "PacienteId",
                principalTable: "Pacientes",
                principalColumn: "PacienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pacientes_Consultas_ConsultasConsultaId",
                table: "Pacientes",
                column: "ConsultasConsultaId",
                principalTable: "Consultas",
                principalColumn: "ConsultaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Planos_de_Saude_Consultas_ConsultasConsultaId",
                table: "Planos_de_Saude",
                column: "ConsultasConsultaId",
                principalTable: "Consultas",
                principalColumn: "ConsultaId");
        }
    }
}
