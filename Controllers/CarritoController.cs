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

        // Método para mostrar la página del carrito
        public IActionResult Index()
        {
            return View(Carrito.carrito); // Pasamos los productos del carrito a la vista
        }

        [HttpPost]
        public IActionResult AgregarAlCarrito(int idProducto)
        {
            // Buscar el producto en la base de datos
            var producto = _appDbContext.Productos.FirstOrDefault(p => p.Id == idProducto);

            if (producto != null)
            {
                // Verificar si el producto ya está en el carrito
                var productoEnCarrito = Carrito.carrito.FirstOrDefault(p => p.Id == idProducto);

                if (productoEnCarrito != null)
                {
                    // Incrementar la cantidad solo si hay stock disponible
                    if (productoEnCarrito.Cantidad < producto.Stock)
                    {
                        productoEnCarrito.Cantidad++;
                    }
                }
                else
                {
                    // Agregar un nuevo producto al carrito con cantidad inicial de 1
                    Carrito.carrito.Add(new Carrito
                    {
                        Id = producto.Id,
                        Nombre = producto.Nombre,
                        Descripcion = producto.Descripcion,
                        Precio = producto.Precio,
                        Categoria = producto.Categoria,
                        Stock = producto.Stock,
                        IconoFontAwesome = producto.IconoFontAwesome,
                        Cantidad = 1 // Inicia con cantidad 1
                    });
                }
            }

            // Redirigir al índice u otra página
            return RedirectToAction("Index", "Productos");
        }

        // Método para eliminar un producto del carrito
        public IActionResult EliminarDelCarrito(int id)
        {
            var producto = Carrito.carrito.FirstOrDefault(p => p.Id == id);
            if (producto != null)
            {
                Carrito.carrito.Remove(producto); // Eliminar del carrito si existe
            }

            return RedirectToAction("Index"); // Volver a la página del carrito
        }

        [HttpPost]
        public JsonResult ModificarCantidadAjax(int idProducto, string modificar)
        {
            var producto = Carrito.carrito.FirstOrDefault(p => p.Id == idProducto);

            if (producto == null)
            {
                return Json(new { success = false, message = "Producto no encontrado." });
            }

            if (modificar == "mas" && producto.Cantidad < producto.Stock)
            {
                producto.Cantidad++;
            }
            else if (modificar == "menos" && producto.Cantidad > 1)
            {
                producto.Cantidad--;
            }
            else
            {
                return Json(new { success = false, message = "Acción no válida o stock insuficiente." });
            }

            // Calcular el nuevo total del carrito
            var totalCompra = Carrito.carrito.Sum(p => p.Precio * p.Cantidad);

            return Json(new
            {
                success = true,
                nuevaCantidad = producto.Cantidad,
                totalCompra = totalCompra
            });
        }
    }
}
