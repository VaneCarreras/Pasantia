using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pasantia.Migrations
{
    /// <inheritdoc />
    public partial class VistaMarca : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductoID",
                table: "Marca",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Producto_MarcaID",
                table: "Producto",
                column: "MarcaID");

            migrationBuilder.AddForeignKey(
                name: "FK_Producto_Marca_MarcaID",
                table: "Producto",
                column: "MarcaID",
                principalTable: "Marca",
                principalColumn: "MarcaID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Producto_Marca_MarcaID",
                table: "Producto");

            migrationBuilder.DropIndex(
                name: "IX_Producto_MarcaID",
                table: "Producto");

            migrationBuilder.DropColumn(
                name: "ProductoID",
                table: "Marca");
        }
    }
}
