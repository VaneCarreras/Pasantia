using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Pasantia.Data;
using Pasantia.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Pasantia.Controllers
{
    public class CategoriasController : Controller
    {
        private ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public CategoriasController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
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

            var categorias = _context.Categoria.ToList();

            return View(categorias.ToList());
        }

        public JsonResult BuscarTodasCategorias()
        {
            var categorias = _context.Categoria.ToList();
            List<Categoria> categoriasMostrar = new List<Categoria>();
            foreach (var categoria in categorias.OrderBy(c => c.CategoriaNombre))
            {
                var categoriaMostrar = new Categoria()
                {
                    CategoriaID = categoria.CategoriaID,
                    CategoriaNombre = categoria.CategoriaNombre,
                    Eliminado = categoria.Eliminado
                };
                categoriasMostrar.Add(categoriaMostrar);
            }
            return Json(categoriasMostrar);
        }

        public JsonResult DeshabilitarCategoria(int id)
        {
            int resultado = 1;

            Categoria categoria = _context.Categoria.Find(id);

            if (id > 0)
            {
                //VALIDAR SI NO TIENE PRODUCTOS ACTIVOS RELACIONADOS---- NO CREE PRODUCTOS AUN
                // var productosActivos = (from o in _context.Categoria where o.CategoriaID == categoria.CategoriaID select o).Count();
                // if (productosActivos > 0 )
                // {
                //      resultado = 0;
                // }
                // else
                {
                categoria.Eliminado = true;
                _context.SaveChanges();
                }
            }

            return Json(resultado);
        }


        public ActionResult HabilitarCategoria(int id)
        {
            Categoria categoria = _context.Categoria.Find(id);

            categoria.Eliminado = false;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        public JsonResult GuardarCategoria(string CategoriaNombre)
        {
            List<string> resultado = new List<string>();

            if (!String.IsNullOrEmpty(CategoriaNombre))
            {
                CategoriaNombre = CategoriaNombre.ToUpper();
                var existeCategoria = _context.Categoria.Where(x => x.CategoriaNombre == CategoriaNombre).ToList();

                if (existeCategoria.Count > 0)
                {
                    resultado.Add("error_CategoriaNombre_existe");
                }
                else
                {
                    Categoria categoria = new Categoria
                    {
                        CategoriaNombre = CategoriaNombre,
                        Eliminado = false
                    };
                    _context.Categoria.Add(categoria);
                    _context.SaveChanges();
                    resultado.Add("correcto");
                }

            }
            if (String.IsNullOrEmpty(CategoriaNombre)) resultado.Add("error_CategoriaNombre");

            return Json(resultado);
        }

        public JsonResult GuardarEditarCategoria(int CategoriaID, string CategoriaNombre)
        {
            //PRIMERO BUSCAMOS EL USUARIO ACTUAL
            var usuarioActual = _userManager.GetUserId(HttpContext.User);
            List<string> resultado = new List<string>();

            if (CategoriaID != 0)
            {
                var categoria = (from a in _context.Categoria where a.CategoriaID == CategoriaID select a).SingleOrDefault();

                if (categoria != null)
                {
                    if (!String.IsNullOrEmpty(CategoriaNombre))
                    {
                        CategoriaNombre = CategoriaNombre.ToUpper();
                        var existeCategoria = _context.Categoria.Where(x => x.CategoriaNombre == CategoriaNombre).ToList();

                        if (existeCategoria.Count > 0)
                        {
                            resultado.Add("error_CategoriaNombre_existe");
                        }
                        else
                        {
                            categoria.Eliminado = false;
                            categoria.CategoriaNombre = CategoriaNombre;
                            _context.SaveChanges();
                            resultado.Add("correcto");
                        }
                    }
                    if (String.IsNullOrEmpty(CategoriaNombre)) resultado.Add("error_CategoriaNombre");
                }
                else
                {
                    resultado.Add("error");
                }
            }

            return Json(resultado);
        }

        public JsonResult BuscarCategoria(int CategoriaID)
        {
            var resultado = new Categoria();

            if (CategoriaID != 0)
            {
                Categoria categoria = _context.Categoria.Find(CategoriaID);

                resultado.CategoriaID = categoria.CategoriaID;
                resultado.CategoriaNombre = categoria.CategoriaNombre;
            }

            return Json(resultado);
        }


        // public JsonResult BuscarCategoriasProductos()
        // {
        //     var categorias = _context.Categoria.Include(c => c.ProductoCategorias).ToList();
        //     List<CategoriaMostrar> categoriasMostrar = new List<CategoriaMostrar>();
        //     foreach (var categoria in categorias.OrderBy(c => c.CategoriaNombre))
        //     {
        //         var categoriaMostrar = new CategoriaMostrar()
        //         {
        //             CategoriaID = categoria.CategoriaID,
        //             CategoriaNombre = categoria.CategoriaNombre,
        //             CantidadProducto = categoria.ProductoCategorias.Count
        //         };
        //         categoriasMostrar.Add(categoriaMostrar);
        //     }

        //     return Json(categoriasMostrar);
        // }

    }
}
