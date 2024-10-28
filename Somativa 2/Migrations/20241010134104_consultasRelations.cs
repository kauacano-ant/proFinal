using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Somativa_2.Migrations
{
    /// <inheritdoc />
    public partial class consultasRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Remova ou comente a linha que tenta remover a chave estrangeira
            // migrationBuilder.DropForeignKey(
            //     name: "FK_Consultas_Pacientes_PacientesPacienteId",
            //     table: "Consultas");

            //migrationBuilder.DropIndex(
            //    name: "IX_Consultas_PacientesPacienteId",
            //    table: "Consultas");

            //migrationBuilder.DropColumn(
            //    name: "PacientesPacienteId",
            //    table: "Consultas");

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_PacienteId",
                table: "Consultas",
                column: "PacienteId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Consultas_Pacientes_PacienteId",
            //    table: "Consultas",
            //    column: "PacienteId",
            //    principalTable: "Pacientes",
            //    principalColumn: "PacienteId",
            //    onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_Pacientes_PacienteId",
                table: "Consultas");

            migrationBuilder.DropIndex(
                name: "IX_Consultas_PacienteId",
                table: "Consultas");

            migrationBuilder.AddColumn<Guid>(
                name: "PacientesPacienteId",
                table: "Consultas",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_PacientesPacienteId",
                table: "Consultas",
                column: "PacientesPacienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_Pacientes_PacientesPacienteId",
                table: "Consultas",
                column: "PacientesPacienteId",
                principalTable: "Pacientes",
                principalColumn: "PacienteId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
