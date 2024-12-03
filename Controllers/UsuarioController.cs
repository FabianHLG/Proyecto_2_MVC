using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto_2_MVC.Data;
using Proyecto_2_MVC.Models;

namespace Proyecto_2_MVC.Controllers
{
    public class UsuarioController : Controller
    {

        private readonly AppDbContext _appDbContext;

        public UsuarioController(AppDbContext context)
        {
            _appDbContext = context;
        }

        // Acción para la vista de registro
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registro(Usuarios usuario)
        {
            if (ModelState.IsValid)
            {
                // Verificar si el email ya existe en la base de datos
                var existingUser = _appDbContext.Usuarios.SingleOrDefault(u => u.Email == usuario.Email);

                if (existingUser != null)
                {
                    // Si el correo ya está registrado, se agrega un mensaje de error
                    ModelState.AddModelError("Email", "El correo electrónico ya está registrado. Por favor, usa otro correo.");
                    return View(usuario);
                }

                usuario.FechaRegistro = DateTime.Now;

                _appDbContext.Usuarios.Add(usuario);
                _appDbContext.SaveChanges();

                return RedirectToAction("Login", "Usuario");
            }

            return View(usuario);
        }


        // Acción para la vista de inicio de sesión
        public IActionResult Login()
        {
            return View();
        }

        // Procesa el inicio de sesión
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            // Busca al usuario en la base de datos
            var user = _appDbContext.Usuarios.SingleOrDefault(u => u.Email == email && u.Contraseña == password);

            if (user != null)
            {
                // Autenticación exitosa: guarda los datos en la sesión
                HttpContext.Session.SetString("UserId", user.Id.ToString());
                HttpContext.Session.SetString("UserNombre", user.Nombre);
                HttpContext.Session.SetString("UserEmail", user.Email);

                // Redirige a la página principal
                return RedirectToAction("Index", "Productos");
            }

            // Manejo de error en caso de credenciales incorrectas
            ModelState.AddModelError("", "Correo o contraseña incorrectos");
            return View();
        }

        public IActionResult Perfil()
        {
            // Verificar si el usuario ha iniciado sesión
            var userId = HttpContext.Session.GetString("UserId");
            if (userId == null)
            {
                // Redirige al inicio de sesión si no está autenticado
                return RedirectToAction("Login", "Usuario");
            }

            // Obtener datos del usuario
            var usuario = _appDbContext.Usuarios.FirstOrDefault(u => u.Id == int.Parse(userId));
            if (usuario == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            // Obtener el historial de compras del usuario
            var historial = _appDbContext.Pedidos
                .Where(p => p.IdUsuario == usuario.Id)
                .GroupBy(p => p.NumeroPedido)
                .Select(grupo => new
                {
                    NumeroPedido = grupo.Key,
                    Fecha = grupo.FirstOrDefault().Fecha,
                    Estado = grupo.FirstOrDefault().Estado,
                    Productos = grupo.Select(p => new
                    {
                        Nombre = _appDbContext.Productos.FirstOrDefault(prod => prod.Id == p.IdProducto).Nombre,
                        Cantidad = p.CantCompra,
                        PrecioTotal = p.PrecioTotal
                    }),
                    TotalCompra = grupo.Sum(p => p.PrecioTotal),
                    TiempoCancelacionRestante = 24 - (DateTime.Now - grupo.FirstOrDefault().Fecha).TotalHours // Tiempo restante en horas
                })
                .ToList();

            // Pasar los datos a la vista
            ViewBag.Usuario = usuario;
            ViewBag.Historial = historial;

            return View();
        }

		[HttpPost]
		public IActionResult ActualizarPerfil(int id, string nombre, string email, string contrasena, DateTime fechaNacimiento, string direccion, string telefono)
		{
			// Buscar al usuario en la base de datos por ID
			var usuario = _appDbContext.Usuarios.FirstOrDefault(u => u.Id == id);

			if (usuario != null)
			{
				// Actualizar los datos del usuario
				usuario.Nombre = nombre;
				usuario.Email = email;
				usuario.Contraseña = contrasena;
				usuario.FechaNacimiento = fechaNacimiento;
				usuario.DireccionExacta = direccion;
				usuario.Telefono = telefono;

				// Guardar los cambios en la base de datos
				_appDbContext.SaveChanges();

				// Actualizar la información de sesión si es necesario
				HttpContext.Session.SetString("UserNombre", nombre);
				HttpContext.Session.SetString("UserEmail", email);
			}

			// Redirigir al perfil del usuario después de actualizar
			return RedirectToAction("Perfil");
		}

        [HttpPost]
        public IActionResult CancelarPedido(string numeroPedido)
        {
            var pedidos = _appDbContext.Pedidos
                .Where(p => p.NumeroPedido == numeroPedido)
                .ToList();

            if (pedidos.Any())
            {
                // Restaurar el stock de cada producto en el pedido
                foreach (var pedido in pedidos)
                {
                    var producto = _appDbContext.Productos.FirstOrDefault(p => p.Id == pedido.IdProducto);
                    if (producto != null)
                    {
                        producto.Stock += pedido.CantCompra; // Devolver la cantidad al stock
                    }
                    pedido.Estado = "Cancelado"; // Actualizar estado del pedido
                }

                _appDbContext.SaveChanges();
            }
            TempData["Mensaje"] = "El pedido fue cancelado y los productos fueron devueltos al inventario.";
            return RedirectToAction("Perfil");
        }

        // Acción de Logout
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Json(new { success = true });
        }
    }
}
