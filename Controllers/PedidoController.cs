using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto_2_MVC.Data;
using Proyecto_2_MVC.Models;

namespace Proyecto_2_MVC.Controllers
{
    public class PedidoController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public PedidoController(AppDbContext context)
        {
            _appDbContext = context;
        }

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

        [HttpPost]
        public IActionResult ConfirmarCompra(string metodoPago)
        {
            var userId = HttpContext.Session.GetString("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            var productosEnCarrito = Carrito.carrito;

            if (productosEnCarrito.Count == 0)
            {
                return RedirectToAction("Index", "Carrito");
            }

            decimal total = 0;
            string numeroDocumento = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();

            foreach (var producto in productosEnCarrito)
            {
                total += producto.Precio * producto.Cantidad;

                // Buscar el producto en la base de datos para ajustar el stock
                var productoEnDb = _appDbContext.Productos.FirstOrDefault(p => p.Id == producto.Id);
                if (productoEnDb != null)
                {
                    if (productoEnDb.Stock >= producto.Cantidad)
                    {
                        productoEnDb.Stock -= producto.Cantidad; // Reducir el stock
                    }
                    else
                    {
                        // Si el stock no es suficiente, mostrar un error
                        TempData["Error"] = $"El producto {productoEnDb.Nombre} no tiene suficiente stock.";
                        return RedirectToAction("Index", "Carrito");
                    }
                }

                // Guardar en la base de datos el pedido
                _appDbContext.Pedidos.Add(new Pedidos
                {
                    IdUsuario = int.Parse(userId),
                    IdProducto = producto.Id,
                    CantCompra = producto.Cantidad,
                    PrecioTotal = producto.Precio * producto.Cantidad,
                    Estado = "Pagado",
                    Fecha = DateTime.Now,
                    NumeroPedido = numeroDocumento
                });
            }

            _appDbContext.SaveChanges();

            // Vaciar el carrito
            Carrito.carrito.Clear();

            // Redirigir a la página de confirmación
            return RedirectToAction("Confirmacion", new { metodoPago, total, numeroDocumento });
        }

        public IActionResult Confirmacion(string metodoPago, decimal total, string numeroDocumento)
        {

            // Obtiene los datos de los productos del pedido desde la base de datos
            var productosDelPedido = _appDbContext.Pedidos
                .Where(p => p.NumeroPedido == numeroDocumento) // Filtrar por el número de pedido
                .Join(
                    _appDbContext.Productos, // Unir con la tabla de productos
                    pedido => pedido.IdProducto,
                    producto => producto.Id,
                    (pedido, producto) => new
                    {
                        producto.IconoFontAwesome,
                        producto.Nombre,
                        pedido.CantCompra,
                        Subtotal = pedido.CantCompra * producto.Precio
                    }
                )
                .ToList();

            ViewBag.MetodoPago = metodoPago;
            ViewBag.Total = total;
            ViewBag.NumeroDocumento = numeroDocumento;
            ViewBag.Productos = productosDelPedido;
            return View();
        }
    }
}
