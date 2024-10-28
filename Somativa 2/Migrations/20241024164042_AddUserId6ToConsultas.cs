using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Somativa_2.Migrations
{
    /// <inheritdoc />
    public partial class AddUserId6ToConsultas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_Consultorios_ConsultorioId",
                table: "Consultas");

            migrationBuilder.AlterColumn<Guid>(
                name: "ConsultorioId",
                table: "Consultas",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_Consultorios_ConsultorioId",
                table: "Consultas",
                column: "ConsultorioId",
                principalTable: "Consultorios",
                principalColumn: "ConsultorioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_Consultorios_ConsultorioId",
                table: "Consultas");

            migrationBuilder.AlterColumn<Guid>(
                name: "ConsultorioId",
                table: "Consultas",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_Consultorios_ConsultorioId",
                table: "Consultas",
                column: "ConsultorioId",
                principalTable: "Consultorios",
                principalColumn: "ConsultorioId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
