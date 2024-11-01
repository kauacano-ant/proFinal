using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Somativa_2.Migrations
{
    /// <inheritdoc />
    public partial class delettotal1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_Planos_de_Saude_ConsultasConsultaId",
                table: "Planos_de_Saude",
                column: "ConsultasConsultaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Planos_de_Saude_Consultas_ConsultasConsultaId",
                table: "Planos_de_Saude",
                column: "ConsultasConsultaId",
                principalTable: "Consultas",
                principalColumn: "ConsultaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Planos_de_Saude_Consultas_ConsultasConsultaId",
                table: "Planos_de_Saude");

            migrationBuilder.DropIndex(
                name: "IX_Planos_de_Saude_ConsultasConsultaId",
                table: "Planos_de_Saude");

            migrationBuilder.DropColumn(
                name: "ConsultaId",
                table: "Planos_de_Saude");

            migrationBuilder.DropColumn(
                name: "ConsultasConsultaId",
                table: "Planos_de_Saude");
        }
    }
}
