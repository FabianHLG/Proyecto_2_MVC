using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto_2_MVC.Models;

namespace Proyecto_2_MVC.Controllers
{
    public class PedidoController : Controller
    {
        public IActionResult Index()
        {
            // Verificar si el usuario ha iniciado sesión
            var userId = HttpContext.Session.GetString("UserId");

            if (userId == null)
            {
                // Redirige al usuario al inicio de sesión si no está autenticado
                return RedirectToAction("Login", "Usuario");
            }

            var productosEnCarrito = Carrito.carrito;
            return View(productosEnCarrito);
        }
    }
}
