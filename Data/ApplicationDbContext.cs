using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Pasantia.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {

    }
        public DbSet<Pasantia.Models.Categoria> Categoria { get; set; }
        public DbSet<Pasantia.Models.Marca> Marca { get; set; }
        public DbSet<Pasantia.Models.Producto> Producto { get; set; }
        public DbSet<Pasantia.Models.ImagenProducto> ImagenProducto { get; set; }
}
