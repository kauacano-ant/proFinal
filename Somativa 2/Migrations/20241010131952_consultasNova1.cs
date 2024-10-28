using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Somativa_2.Migrations
{
    /// <inheritdoc />
    public partial class consultasNova1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Apenas remova a referência à coluna se ela não existir
            // Se a coluna não existe, não há nada a ser feito aqui
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Se você quiser reverter, pode adicionar a coluna de volta
            migrationBuilder.AddColumn<Guid>(
                name: "PacientesPacienteId",
                table: "Consultas",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
