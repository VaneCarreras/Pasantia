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

        public virtual ICollection<ImagenProducto> ImagenProducto {  get; set; }
        public virtual Marca Marca { get; set; }

    }


    
}
