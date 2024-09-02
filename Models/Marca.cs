using System.ComponentModel.DataAnnotations;

namespace Pasantia.Models
{
    public class Marca
    {
        [Key]
        public int MarcaID { get; set; }

        public string? MarcaNombre { get; set; }

        public bool Eliminado { get; set; }
    }

    
}
