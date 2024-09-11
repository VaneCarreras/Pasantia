using System.ComponentModel.DataAnnotations;

namespace Pasantia.Models
{
    public class Producto
    {
        [Key]
        public int ProductoID { get; set; }

        public string? Descripcion { get; set; }

        public int MarcaID { get; set; }

        public int CategoriaID { get; set; }

        public bool Eliminado { get; set; }

        public virtual ICollection<ImagenProducto> ImagenesProducto {  get; set; }
        public virtual Marca Marca { get; set; }
        public virtual Categoria Categoria { get; set; }

    }

    public class ProductoMostrar
    {
        public int ProductoID { get; set; }
        public string? Descripcion { get; set; }
        public int MarcaID { get; set; }
        public string? MarcaNombre { get; set; }
        public int CantidadImagenes { get; set; }
        public string? Base64 { get; set; }
        public List<string> NombreCategorias { get; set; }
        public List<string> ImagenesProductos { get; set; }
    }

    public class ComboGeneral
    {
        public int ID { get; set; }

        public string? Descripcion { get; set; }
    }

    
}
