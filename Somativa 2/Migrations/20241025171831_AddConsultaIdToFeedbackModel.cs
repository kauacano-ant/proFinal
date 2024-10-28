using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Somativa_2.Migrations
{
    /// <inheritdoc />
    public partial class AddConsultaIdToFeedbackModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_feedback_Consultas_ConsultaId",
                table: "feedback");

            migrationBuilder.AlterColumn<Guid>(
                name: "ConsultaId",
                table: "feedback",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_feedback_Consultas_ConsultaId",
                table: "feedback",
                column: "ConsultaId",
                principalTable: "Consultas",
                principalColumn: "ConsultaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_feedback_Consultas_ConsultaId",
                table: "feedback");

            migrationBuilder.AlterColumn<Guid>(
                name: "ConsultaId",
                table: "feedback",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_feedback_Consultas_ConsultaId",
                table: "feedback",
                column: "ConsultaId",
                principalTable: "Consultas",
                principalColumn: "ConsultaId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
