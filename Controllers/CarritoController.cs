using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_2_MVC.Data;
using Proyecto_2_MVC.Models;
using System.Diagnostics;

namespace Proyecto_2_MVC.Controllers
{
    public class CarritoController : Controller
    {

        private readonly AppDbContext _appDbContext;

        public CarritoController(AppDbContext context)
        {
            _appDbContext = context;
        }

        // M�todo para mostrar la p�gina del carrito
        public IActionResult Index()
        {
            return View(Carrito.carrito); // Pasamos los productos del carrito a la vista
        }

        // M�todo para a�adir un producto al carrito
        [HttpPost]
        public IActionResult AgregarAlCarrito(int id)
        {
            // Buscar el producto en la base de datos
            var producto = _appDbContext.Productos.FirstOrDefault(p => p.Id == id);

            if (producto != null)
            {
                Carrito.carrito.Add(producto); // Agregar al carrito si existe
            }

            return RedirectToAction("Index", "Productos"); // Redirigir a la p�gina principal
        }

        // M�todo para eliminar un producto del carrito
        public IActionResult EliminarDelCarrito(int id)
        {
            var producto = Carrito.carrito.FirstOrDefault(p => p.Id == id);
            if (producto != null)
            {
                Carrito.carrito.Remove(producto); // Eliminar del carrito si existe
            }

            return RedirectToAction("Index"); // Volver a la p�gina del carrito
        }
    }
}
