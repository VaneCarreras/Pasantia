using System.ComponentModel.DataAnnotations;

namespace Pasantia.Models
{
    public class Marca
    {
        [Key]
        public int MarcaID { get; set; }

        public string? MarcaNombre { get; set; }

        public bool Eliminado { get; set; }

        public virtual ICollection<Producto> Productos { get; set; }
    }

    public class MarcaMostrar
    {
        public int MarcaID { get; set; }
        public string? MarcaNombre { get; set; }
        public bool Eliminado { get; set; }
        public int CantidadProductos { get; set; }
        public int ProductoID { get; set; }
        public string? ProductoNombre { get; set; }
    }
}
