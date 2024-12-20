using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Pasantia.Data;
using Pasantia.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace Pasantia.Controllers
{
    public class ProductosController : Controller
    {
        private ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public ProductosController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize]
        public ActionResult Index(string ErrorEliminar)
        {
            var productos = _context.Producto.ToList();

            if (ErrorEliminar != null)
            {
                ViewBag.ErrorEliminar = ErrorEliminar;
            }

            //combogralxcondicion

            var marcas = (from m in _context.Marca
              where m.Eliminado == false  // Filtramos las marcas no eliminadas
              select m).ToList();
            marcas.Add(new Marca { MarcaID = 0, MarcaNombre = "[SELECCIONE MARCA]" });
            marcas = marcas.OrderBy(a => a.MarcaNombre).ToList();
            ViewBag.MarcaID = new SelectList(marcas, "MarcaID", "MarcaNombre");

            var categorias = (from c in _context.Categoria
              where c.Eliminado == false  // Filtramos las marcas no eliminadas
              select c).ToList();
            categorias.Add(new Categoria { CategoriaID = 0, CategoriaNombre = "[SELECCIONE CATEGORÍA]" });
            categorias = categorias.OrderBy(a => a.CategoriaNombre).ToList();
            ViewBag.CategoriaID = new SelectList(categorias, "CategoriaID", "CategoriaNombre");

            return View();
        }

        public JsonResult BuscarTodosProductos()
        {
            var productosCargados = (from cam in _context.Producto select cam).ToList();

            List<ProductoMostrar> listaDeProductos = new List<ProductoMostrar>();
            foreach (var p in productosCargados.OrderBy(c => c.Descripcion))
            {
                var marcaNombre = (from c in _context.Marca where c.MarcaID == p.MarcaID select c.MarcaNombre).Single();
                var categoriaNombre = (from c in _context.Categoria where c.CategoriaID == p.CategoriaID select c.CategoriaNombre).Single();

                //VARIABLE DE LA COND DE USADO O REACOND

                var registro = new ProductoMostrar
                {
                    ProductoID = p.ProductoID,
                    Descripcion = p.Descripcion,
                    MarcaNombre = marcaNombre,
                    CategoriaNombre = categoriaNombre
                };
                listaDeProductos.Add(registro);
            }
            return Json(listaDeProductos);
        }

        public JsonResult EliminarProducto(int id)
        {
            Producto producto = _context.Producto.Find(id);
            if (producto != null)
            {
                _context.Producto.Remove(producto);
                _context.SaveChanges();
            }

            var resultado = "Eliminado correctamente.";

            return Json(resultado);
        }


        public JsonResult BuscarProducto(int ProductoID)
        {
            var resultado = new ProductoMostrar();

            if (ProductoID != 0)
            {
                Producto producto = _context.Producto.Find(ProductoID);

                var marcaNombre = (from c in _context.Marca where c.MarcaID == producto.MarcaID select c.MarcaNombre).Single();
                var categoriaNombre = (from c in _context.Categoria where c.CategoriaID == producto.CategoriaID select c.CategoriaNombre).Single();

                resultado.ProductoID = ProductoID;
                resultado.Descripcion = producto.Descripcion;
                resultado.MarcaID = producto.MarcaID;
                resultado.MarcaNombre = marcaNombre;
                resultado.CategoriaID = producto.CategoriaID;
                resultado.CategoriaNombre = categoriaNombre;
            }

            return Json(resultado);
        }

        public JsonResult GuardarProducto(string Descripcion,
            int MarcaID, int CategoriaID)
        {
            List<string> resultado = new List<string>();

            if (!String.IsNullOrEmpty(Descripcion))
            {
                Descripcion = Descripcion.ToUpper();

                //COMPROB DE ESTADO

                //COMPROB DETALLE

                //COMPROB CODIGO

                Producto producto = new Producto
                {
                    Descripcion = Descripcion,
                    MarcaID = MarcaID,
                    CategoriaID = CategoriaID
                };
                _context.Producto.Add(producto);
                _context.SaveChanges();
                resultado.Add("correcto");

            }
            if (String.IsNullOrEmpty(Descripcion)) resultado.Add("error_ProductoNombre");

            return Json(resultado);
        }

        // public JsonResult GuardarProducto(string Descripcion, string Codigo,
        //             int MarcaID, int Estado, string Detalle)
        //         {
        //             List<string> resultado = new List<string>();

        //             if (!String.IsNullOrEmpty(Descripcion))
        //             {
        //                 Descripcion = Descripcion.ToUpper();

        //                 if (Estado < 1 || Estado > 5)
        //                 {
        //                     resultado.Add("error_EstadoFueraDeRango");
        //                     return Json(resultado);
        //                 }

        //                 string detalle = "";
        //                 if (!String.IsNullOrEmpty(Detalle))
        //                 {
        //                     detalle = Detalle.ToUpper();
        //                 }

        //                 string codigoProducto = "";
        //                 if (!String.IsNullOrEmpty(Codigo))
        //                 {
        //                     codigoProducto = Codigo.ToUpper();
        //                 }

        //                 Producto producto = new Producto
        //                 {
        //                     Descripcion = Descripcion,
        //                     MarcaID = MarcaID
        //                 };
        //                 _context.Producto.Add(producto);
        //                 _context.SaveChanges();
        //                 resultado.Add("correcto");

        //             }
        //             if (String.IsNullOrEmpty(Descripcion)) resultado.Add("error_ProductoNombre");

        //             return Json(resultado);
        //         }


        public JsonResult GuardarEditarProducto(int ProductoID, string Descripcion,
            int MarcaID, int CategoriaID)
        {
            //PRIMERO BUSCAMOS EL USUARIO ACTUAL
            var usuarioActual = _userManager.GetUserId(HttpContext.User);
            List<string> resultado = new List<string>();

            if (ProductoID != 0)
            {
                var producto = (from a in _context.Producto where a.ProductoID == ProductoID select a).SingleOrDefault();

                if (producto != null)
                {
                    if (!String.IsNullOrEmpty(Descripcion))
                    {
                        Descripcion = Descripcion.ToUpper();
                    }

                    producto.Descripcion = Descripcion;
                    producto.MarcaID = MarcaID;
                    producto.CategoriaID = CategoriaID;

                    _context.SaveChanges();
                    resultado.Add("correcto");

                    if (String.IsNullOrEmpty(Descripcion)) resultado.Add("error_ProductoNombre");
                }
                else
                {
                    resultado.Add("error");
                }
            }

            return Json(resultado);
        }

        //BUSCAR CATEGORIAS METODO CE RECUPERACION

        // public JsonResult AgregarCategorias(int CategoriaID, int ProductoID)
        // {
        //     bool resultado = false;

        //     //FIJAR INFORMACION DE CULTURA PARA FECHA Y DECIMALES
        //     Thread.CurrentThread.CurrentCulture = new CultureInfo("es-AR");

        //     if (ProductoID != 0)
        //     {
        //         var producto = (from o in _context.Producto where o.ProductoID == ProductoID select o)
        //             .Include(c => c.Categoria).Single();
        //         if (producto != null)
        //         {
        //             if (CategoriaID > 0)
        //             {
        //                 var existeCategoria = _context.Categoria
        //                     .Where(x => x.CategoriaID == CategoriaID).ToList();
        //                 if (existeCategoria.Count > 0)
        //                 {
        //                     resultado = false;
        //                 }
        //                 else
        //                 {
        //                     var categoriaProductos = new ProductoCategoria
        //                     {
        //                         CategoriaID = CategoriaID,
        //                         ProductoID = ProductoID
        //                     };
        //                     _context.SaveChanges();

        //                     resultado = true;
        //                 }

        //             }
        //         }

        //     }

        //     return Json(resultado);
        // }




        // public JsonResult QuitarCategoria(ProductoCategoria)
        // {
        //     bool resultado = false;

        //     if (CategoriaID > 0)
        //     {
        //         var productoCategoria = (from o in _context.Categoria.ProductoCategoria where o.CategoriaID == CategoriaID select o).Include(p => p.Producto).Single();

        //         if (productoCategoria != null)
        //         {
        //             _context.Categoria.ProductoCategoria.Remove(productoCategoria);
        //             _context.SaveChanges();
        //             resultado = true;
        //         }
        //     }

        //     return Json(resultado);
        // }


        public JsonResult BuscarImagenes(int ProductoID)
        {
            List<VistaImagenProducto> listaImagenProducto = new List<VistaImagenProducto>();

            var imagenesProd = (from o in _context.ImagenProducto where o.ProductoID == ProductoID select o).ToList();

            foreach (var item in imagenesProd)
            {
                string returnValue = System.Convert.ToBase64String(item.Imagen);

                var imagenProducto = new VistaImagenProducto
                {
                    ImgProductosID = item.ImagenProductoID,
                    Base64 = returnValue
                };
                listaImagenProducto.Add(imagenProducto);
            }

            return Json(listaImagenProducto);
        }

        public JsonResult GuardarImagen(string ImagenAGuardar, int ProductoID)
        {
            var resultado = false;

            try
            {
                var cantidadImagenes = (from o in _context.ImagenProducto where o.ProductoID == ProductoID select o).Count();
                if (cantidadImagenes < 3)
                {
                    if (ImagenAGuardar != null && ImagenAGuardar.Length > 0)
                    {
                        byte[] bytes = Convert.FromBase64String(ImagenAGuardar.Split(',')[1]);

                        var imagenProd = new ImagenProducto
                        {
                            Imagen = bytes,
                            ProductoID = Convert.ToInt32(ProductoID)
                        };
                        _context.ImagenProducto.Add(imagenProd);
                        _context.SaveChanges();

                        resultado = true;
                    }
                }
                else
                {
                    resultado = false;
                }
            }
            catch (Exception ex)
            {
                resultado = false;
            }

            return Json(resultado);
        }

        public JsonResult EliminarImagenProducto(int ImgProductosID)
        {
            bool resultado = true;

            ImagenProducto imagenProducto = _context.ImagenProducto.Find(ImgProductosID);

            _context.ImagenProducto.Remove(imagenProducto);
            _context.SaveChanges();

            return Json(resultado);
        }


    











// //////////////////////VERVERVERVEVRVEVRVEVRVEVR





        public JsonResult BuscarProductosCatalogo(int MarcaID, int CategoriaID, int pageNumber = 1, int pageSize = 12)
{
    var productosCargados = _context.Producto
        .Include(p => p.ImagenesProducto)
        .Include(p => p.Categoria) // Asegúrate de incluir la categoría aquí
        .AsQueryable();

    if (MarcaID != 0)
    {
        productosCargados = productosCargados.Where(p => p.MarcaID == MarcaID);
    }

    if (CategoriaID != 0)
    {
        productosCargados = productosCargados.Where(p => p.CategoriaID == CategoriaID);
    }

    int totalProductos = productosCargados.Count();

    var productosPaginados = productosCargados
        .OrderBy(p => p.ProductoID)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToList();

    List<ProductoMostrar> listaDeProductos = new List<ProductoMostrar>();

    foreach (var p in productosPaginados)
    {
        var marcaNombre = _context.Marca
            .Where(c => c.MarcaID == p.MarcaID)
            .Select(c => c.MarcaNombre)
            .SingleOrDefault();


        var categoriaNombre = _context.Categoria
            .Where(c => c.CategoriaID == p.CategoriaID)
            .Select(c => c.CategoriaNombre)
            .SingleOrDefault();

        string imagenString = "";

        if (p.ImagenesProducto != null && p.ImagenesProducto.Any())
        {
            imagenString = Convert.ToBase64String(p.ImagenesProducto.First().Imagen);
        }

        var registro = new ProductoMostrar
        {
            ProductoID = p.ProductoID,
            Descripcion = p.Descripcion,
            MarcaNombre = marcaNombre,
            CategoriaNombre = categoriaNombre,
            Base64 = imagenString
        };
        listaDeProductos.Add(registro);
    }

    int totalPaginas = (int)Math.Ceiling((double)totalProductos / pageSize);

    return Json(new { Productos = listaDeProductos, TotalPaginas = totalPaginas });
}

public JsonResult BuscarProductoDetalle(int ProductoID)
{
    var resultado = new ProductoMostrar();

    if (ProductoID != 0)
    {
        var infoProducto = _context.Producto
            .Include(i => i.ImagenesProducto)
            .Include(i => i.Categoria) // Incluir la categoría directamente
            .FirstOrDefault(i => i.ProductoID == ProductoID);

        if (infoProducto != null) // Verificar que el producto exista
        {
            var marcaNombre = _context.Marca
                .Where(c => c.MarcaID == infoProducto.MarcaID)
                .Select(c => c.MarcaNombre)
                .SingleOrDefault();

            List<string> imagenesProductos = new List<string>();
            if (infoProducto.ImagenesProducto != null && infoProducto.ImagenesProducto.Any())
            {
                foreach (var imagenProducto in infoProducto.ImagenesProducto)
                {
                    string imagenString = Convert.ToBase64String(imagenProducto.Imagen);
                    imagenesProductos.Add(imagenString);
                }
            }

            List<string> categoriasProductos = new List<string>
            {
                infoProducto.Categoria.CategoriaNombre // Obtener el nombre de la categoría directamente
            };

            // Asignar datos a resultado
            resultado.ProductoID = ProductoID;
            resultado.Descripcion = infoProducto.Descripcion;
            resultado.MarcaNombre = marcaNombre;
            resultado.ImagenesProductos = imagenesProductos;
            resultado.NombreCategorias = categoriasProductos;
        }
    }

    return Json(resultado);
}

    }
}

// //no esta verificado
//         //public JsonResult BuscarProductosCatalogo_SP(int MarcaID, int CategoriaID, int pageNumber = 1, int pageSize = 9)
//         //{
//         //    Stopwatch timeMeasure = new Stopwatch();
//         //    timeMeasure.Start();

//         //    IEnumerable<ProductoMostrar> listaDeProductos = _context.Database.SqlQuery<ProductoMostrar>(
//         //        @"exec sp_Listado_Productos_Catalogo @marcaID, @categoriaID, @pageNumber, @pageSize",
//         //        new SqlParameter("@marcaID", MarcaID),
//         //        new SqlParameter("@categoriaID", CategoriaID),
//         //        new SqlParameter("@pageNumber", pageNumber),
//         //        new SqlParameter("@pageSize", pageSize)
//         //        ).ToList();

//         //    JsonResult resultado = Json(listaDeProductos);
//         //    return resultado;
//         //}






// //  no se pudo hacer migracion, error con netroles, por eso se comenta, da errores