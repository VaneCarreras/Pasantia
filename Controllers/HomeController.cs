using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pasantia.Data;
using Pasantia.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace Pasantia.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            await InicializarUsuario();
            return View();
        }

        public IActionResult Catalogo()
        {
            return View();
        }

       public ActionResult DetalleProducto(int id)
{
    Producto producto = _context.Producto.Find(id);
    
    if (producto == null)
    {
        // Si el producto es null, redirige a la acción Index
        return RedirectToAction("Index");
    }
    
    // Si el producto no es null, entonces asigna su ID a ViewBag
    ViewBag.Producto = producto.ProductoID;
    
    // Devuelve la vista con el producto encontrado
    return View(producto);
}


        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult InicioSistema()
        {   //BUSCAMOS EL USUARIO ACTUAL
            var usuarioactual = _userManager.GetUserId(HttpContext.User);

            //REDIRECCIONAMOS AL INDEX DE CATEGORÍAS
            return RedirectToAction("Index", "Categorias");
        }

        public async Task<JsonResult> InicializarUsuario()
        {
            // Correo electrónico del usuario
            string email = "admin@aceros.com";

            // Verificamos si el usuario ya existe
            var usuario = await _userManager.FindByEmailAsync(email);

            if (usuario == null)
            {
                // Creamos el usuario si no existe
                var user = new IdentityUser { UserName = email, Email = email };
                var result = await _userManager.CreateAsync(user, "admin.aceros");

                return Json(result.Succeeded);
            }

            return Json(true); // El usuario ya existe
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
   
        // Simulación de productos en la "base de datos"
        private static readonly List<Producto> productos = new List<Producto>
        {
            new Producto { ProductoID = 1}
            // Agrega más productos aquí si lo deseas
        };

        // EndPoint que devuelve la lista de productos
        [HttpGet]
        public IActionResult GetProductos()
        {
            return Ok(productos);
        }
    }

   
}
