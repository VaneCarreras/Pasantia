using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pasantia.Migrations
{
    /// <inheritdoc />
    public partial class QuitarCampoProductoEnMarca : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductoID",
                table: "Marca");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_CategoriaID",
                table: "Producto",
                column: "CategoriaID");

            migrationBuilder.AddForeignKey(
                name: "FK_Producto_Categoria_CategoriaID",
                table: "Producto",
                column: "CategoriaID",
                principalTable: "Categoria",
                principalColumn: "CategoriaID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Producto_Categoria_CategoriaID",
                table: "Producto");

            migrationBuilder.DropIndex(
                name: "IX_Producto_CategoriaID",
                table: "Producto");

            migrationBuilder.AddColumn<int>(
                name: "ProductoID",
                table: "Marca",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
