using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Somativa_2.Migrations
{
    /// <inheritdoc />
    public partial class catg1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CategId",
                table: "Consultorios",
                type: "uniqueidentifier",
                nullable: true,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));


            migrationBuilder.CreateTable(
                name: "Categ",
                columns: table => new
                {
                    CategId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categ", x => x.CategId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Consultorios_CategId",
                table: "Consultorios",
                column: "CategId");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultorios_Categ_CategId",
                table: "Consultorios",
                column: "CategId",
                principalTable: "Categ",
                principalColumn: "CategId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultorios_Categ_CategId",
                table: "Consultorios");

            migrationBuilder.DropTable(
                name: "Categ");

            migrationBuilder.DropIndex(
                name: "IX_Consultorios_CategId",
                table: "Consultorios");

            migrationBuilder.DropColumn(
                name: "CategId",
                table: "Consultorios");
        }
    }
}
