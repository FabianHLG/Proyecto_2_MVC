using Microsoft.AspNetCore.Mvc;
using Proyecto_2_MVC.Data;
using Proyecto_2_MVC.Models;

namespace Proyecto_2_MVC.Controllers
{
    public class ProductosController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public ProductosController(AppDbContext context)
        {
            _appDbContext = context;
        }

        public IActionResult Index()
        {
            ViewBag.CarritoCount = Carrito.carrito.Count; // Pasamos la cantidad al ViewBag

            // Obtener todos los productos desde la base de datos
            var productos = _appDbContext.Productos.ToList();

            // Pasar los productos a la vista
            return View(productos);
        }

        public IActionResult FiltrarPorCategoria(string categoria)
        {
            // Filtrar productos por la categoría seleccionada
            var productos = _appDbContext.Productos
                .Where(p => p.Categoria == categoria)
                .ToList();

            return View("Index", productos); // Retornar a la vista principal con los productos filtrados
        }

        public IActionResult Buscar(string query)
        {
            // Buscar productos que coincidan con el nombre o descripción
            var productos = _appDbContext.Productos
                .Where(p => p.Nombre.Contains(query) || p.Descripcion.Contains(query))
                .ToList();

            if (string.IsNullOrWhiteSpace(query))
            {
                // Obtener todos los productos desde la base de datos
                productos = _appDbContext.Productos.ToList();
            }

            return View("Index", productos); // Retornar a la vista principal con los productos filtrados
        }
        
        public IActionResult Detalles(int id)
        {
            var producto = _appDbContext.Productos.Where(p => p.Id == id)
                .ToList(); // Método que obtiene el producto por ID
            if (producto == null)
            {
                return NotFound(); // Si el producto no existe, se devuelve un error 404
            }
            return View("Detalles", producto);
        }

		[HttpPost]
		public IActionResult AgregarAlCarrito(int idProducto)
		{
			// Obtener el producto por ID
			var producto = _appDbContext.Productos.FirstOrDefault(p => p.Id == idProducto);

			if (producto != null)
			{
				// Agregar el producto al carrito
				Carrito.carrito.Add(producto);
			}

			// Redirigir al índice u otra página
			return RedirectToAction("Index");
		}
	}
}
