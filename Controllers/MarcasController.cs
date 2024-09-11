using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Pasantia.Data;
using Pasantia.Models;
using Microsoft.EntityFrameworkCore;

namespace Pasantia.Controllers
{
    public class MarcasController : Controller
    {
        private ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public MarcasController(ApplicationDbContext context, UserManager<IdentityUser> userManager)


        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize]
        public ActionResult Index(string ErrorEliminar)
        {

            if (ErrorEliminar != null)
            {
                ViewBag.ErrorEliminar = ErrorEliminar;
            }

            return View();
        }

        public JsonResult BuscarTodasMarcas()
        {
            var marcas = _context.Marca.ToList();
            List<MarcaMostrar> marcasMostrar = new List<MarcaMostrar>();
            foreach (var marca in marcas.OrderBy(c => c.MarcaNombre))
            {
                var marcaMostrar = new MarcaMostrar()
                {
                    MarcaID = marca.MarcaID,
                    MarcaNombre = marca.MarcaNombre,
                    Eliminado = marca.Eliminado,
                    //CantidadProductos = marca.Producto.Count
                };
                marcasMostrar.Add(marcaMostrar);
            }

            return Json(marcasMostrar);
        }

        public JsonResult DeshabilitarMarca(int id)
        {
            int resultado = 1;

            Marca marca = _context.Marca.Find(id);

            if (id > 0)
            {
                //VALIDAR SI NO TIENE PRODUCTOS ACTIVOS RELACIONADOS
                var productosActivos = (from o in _context.Producto where o.MarcaID == marca.MarcaID select o).Count();

                if (productosActivos > 0 )
                {
                    //NO SE PUEDE DESHABILITAR LA MARCA.
                    resultado = 0;
                }
                else
                {
                    marca.Eliminado = true;
                    _context.SaveChanges();
                }

            }

            return Json(resultado);
        }

        public ActionResult HabilitarMarca(int id)
        {
            Marca marca = _context.Marca.Find(id);

            marca.Eliminado = false;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public JsonResult GuardarMarca(string MarcaNombre)
        {
            List<string> resultado = new List<string>();

            if (!String.IsNullOrEmpty(MarcaNombre))
            {
                MarcaNombre = MarcaNombre.ToUpper();
                var existeMarca = _context.Marca.Where(x => x.MarcaNombre == MarcaNombre).ToList();

                if (existeMarca.Count > 0)
                {
                    resultado.Add("error_MarcaNombre_existe");
                }
                else
                {
                    Marca marca = new Marca
                    {
                        MarcaNombre = MarcaNombre,
                        Eliminado = false
                    };
                    _context.Marca.Add(marca);
                    _context.SaveChanges();
                    resultado.Add("correcto");
                }

            }
            if (String.IsNullOrEmpty(MarcaNombre)) resultado.Add("error_MarcaNombre");

            return Json(resultado);
        }

        public JsonResult GuardarEditarMarca(int MarcaID, string MarcaNombre)
        {
            //PRIMERO BUSCAMOS EL USUARIO ACTUAL
            var usuarioActual = _userManager.GetUserId(HttpContext.User);
            List<string> resultado = new List<string>();

            if (MarcaID != 0)
            {
                var marca = (from a in _context.Marca where a.MarcaID == MarcaID select a).SingleOrDefault();

                if (marca != null)
                {
                    if (!String.IsNullOrEmpty(MarcaNombre))
                    {
                        MarcaNombre = MarcaNombre.ToUpper();
                        var existeMarca = _context.Marca.Where(x => x.MarcaNombre == MarcaNombre).ToList();

                        if (existeMarca.Count > 0)
                        {
                            resultado.Add("error_MarcaNombre_existe");
                        }
                        else
                        {
                            marca.Eliminado = false;
                            marca.MarcaNombre = MarcaNombre;
                            _context.SaveChanges();
                            resultado.Add("correcto");
                        }
                    }
                    if (String.IsNullOrEmpty(MarcaNombre)) resultado.Add("error_MarcaNombre");
                }
                else
                {
                    resultado.Add("error");
                }
            }

            return Json(resultado);
        }

        public JsonResult BuscarMarca(int MarcaID)
        {
            var resultado = new Marca();

            if (MarcaID != 0)
            {
                Marca marca = _context.Marca.Find(MarcaID);

                resultado.MarcaID = marca.MarcaID;
                resultado.MarcaNombre = marca.MarcaNombre;

            }

            return Json(resultado);
        }

        public JsonResult BuscarMarcasPorDescripcion(string texto)
        {
            List<Marca> marcasMostrar = new List<Marca>();

            if (!string.IsNullOrEmpty(texto))
            {
                texto = texto.ToUpper();
            }

            if (texto.Length > 2)
            {
                var marcasEncontradas = _context.Marca.Where(p => p.MarcaNombre.Contains(texto))
                    .OrderByDescending(p => p.MarcaNombre)
                    .Take(50)
                    .ToList();

                foreach (var marca in marcasEncontradas)
                {
                    var marcaMostrar = new Marca
                    {
                        MarcaID = marca.MarcaID,
                        MarcaNombre = marca.MarcaNombre?.Trim()
                    };
                    marcasMostrar.Add(marcaMostrar);
                }
            }

            return Json(marcasMostrar);
        }
    }
}
