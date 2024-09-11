using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pasantia.Models
{
    public class Categoria
    {
        [Key]
        public int CategoriaID { get; set; }

        public string? CategoriaNombre { get; set; }

        public bool Eliminado { get; set; }

        public virtual ICollection<Producto> Productos { get; set; }
    }

    public class CategoriaMostrar
    {
        public int CategoriaID { get; set; }
        public string? CategoriaNombre { get; set; }
        public int CantidadProducto { get; set; }
    }

    public class ProductoCategoria
    {
        public int CategoriaID {get; set; }
        public int ProductoID { get; set; }
    }
}
