using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_2_MVC.Data;
using Proyecto_2_MVC.Models;
using System.Diagnostics;

namespace Proyecto_2_MVC.Controllers
{
    public class HomeController : Controller
    {

        private readonly AppDbContext _appDbContext;

        public HomeController(AppDbContext context)
        {
            _appDbContext = context;
        }

        public IActionResult Index()
        {
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

    }
}
