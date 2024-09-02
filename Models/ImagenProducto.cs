using System.ComponentModel.DataAnnotations;

namespace Pasantia.Models
{
    public class ImagenProducto
    {
        [Key]
        public int ImagenProductoID { get; set; }

        public byte[] Imagen { get; set; }

        public int ProductoID { get; set; }

        public virtual Producto Producto { get; set; }
    }

    
}
