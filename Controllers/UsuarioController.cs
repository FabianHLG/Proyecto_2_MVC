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

        //// Acción para la vista de perfil
        //public IActionResult Perfil()
        //{
        //    var userId = HttpContext.Session.GetString("UserId");
        //    if (userId == null)
        //    {
        //        return RedirectToAction("Login");
        //    }

        //    var user = _appDbContext.Usuarios.Find(int.Parse(userId));

        //    // Obtener el historial de compras del usuario
        //    var historialCompras = _appDbContext.Reservas
        //        .Where(r => r.UsuarioId == user.Id) // Filtrar reservas por el ID del usuario
        //        .Join(_appDbContext.Rutas, // Hacer join con la tabla Rutas
        //              reserva => reserva.RutaId, // Campo en Reservas
        //              ruta => ruta.Id,           // Campo en Rutas
        //              (reserva, ruta) => new // Crear un nuevo objeto para almacenar la información
        //              {
        //                  Id = reserva.Id,
        //                  Fecha = reserva.FechaReserva,
        //                  Origen = ruta.Origen,
        //                  Destino = ruta.Destino,
        //                  FechaSalida = ruta.Horario,
        //                  PrecioRuta = ruta.Precio,
        //                  Asientos = reserva.AsientoSeleccionado,
        //                  PrecioTotal = reserva.PrecioTotal,
        //                  EstadoPago = reserva.EstadoPago
        //              })
        //        .ToList();

        //    // Pasar el historial al ViewBag
        //    ViewBag.HistorialCompras = historialCompras;

        //    return View(user);
        //}

        //// Procesa la edición del perfil de usuario
        //[HttpPost]
        //public IActionResult EditPerfil(Usuario updatedUser)
        //{

        //    // Encuentra al usuario en la base de datos
        //    var user = _appDbContext.Usuario.Find(updatedUser.Id);

        //    if (user != null)
        //    {
        //        // Actualiza las propiedades del usuario con los nuevos valores
        //        user.Nombre = updatedUser.Nombre;
        //        user.Email = updatedUser.Email;
        //        user.DireccionExacta = updatedUser.DireccionExacta;
        //        user.Telefono = updatedUser.Telefono;
        //        user.FechaNacimiento = updatedUser.FechaNacimiento;

        //        // Guarda los cambios en la base de datos
        //        _appDbContext.SaveChanges();

        //        // Actualiza el nombre del usuario en la sesión
        //        HttpContext.Session.SetString("UserNombre", user.Nombre);

        //        return RedirectToAction("Perfil");
        //    }
        //    return View("Perfil", updatedUser);
        //}

        //[HttpPost]
        //public IActionResult CancelarReserva(int reservaId)
        //{
        //    // Obtener la reserva de la base de datos
        //    var reserva = _appDbContext.Reservas.Find(reservaId);
        //    if (reserva == null)
        //    {
        //        return NotFound("Reserva no encontrada");
        //    }
        //    var reservaSalida = _appDbContext.Rutas.SingleOrDefault(r => r.Id == reserva.RutaId);
        //    if (reservaSalida == null)
        //    {
        //        return NotFound("Reserva no encontrada");
        //    }

        //    var asientosOcupados = _appDbContext.Reservas
        //        .Where(r => r.RutaId == reservaSalida.Id)
        //        .Select(r => r.AsientoSeleccionado)
        //        .ToList();

        //    // Convertir la lista de strings a una lista de enteros
        //    var asientosOcupadosInt = asientosOcupados
        //        .SelectMany(asiento => asiento.Split(','))
        //        .Select(asiento => int.Parse(asiento.Trim()))
        //        .ToList();

        //    // Calcular el tiempo restante hasta la fecha de la ruta
        //    var tiempoRestante = reservaSalida.Horario - DateTime.Now;

        //    // Verificar si el tiempo restante es mayor a 12 horas
        //    if (tiempoRestante.TotalHours > 12)
        //    {
        //        // Permitir la cancelación y eliminar la reserva
        //        _appDbContext.Reservas.Remove(reserva);
        //        _appDbContext.SaveChanges();
        //        TempData["Mensaje"] = "Reserva cancelada exitosamente.";

        //        if (reservaSalida != null)
        //        {
        //            // Actualiza las propiedades de la ruta con los nuevos valores
        //            reservaSalida.Origen = reservaSalida.Origen;
        //            reservaSalida.Destino = reservaSalida.Destino;
        //            reservaSalida.Precio = reservaSalida.Precio;
        //            reservaSalida.AsientosDisponibles = reservaSalida.AsientosDisponibles + asientosOcupadosInt.Count;

        //            // Guarda los cambios en la base de datos
        //            _appDbContext.SaveChanges();
        //        }
        //    }
        //    else
        //    {
        //        // Mostrar mensaje de error si ya no se puede cancelar
        //        TempData["Error"] = "No se puede cancelar la reserva. Está fuera del tiempo permitido.";
        //    }

        //    return RedirectToAction("Perfil");
        //}
        // Acción de Logout
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Json(new { success = true });
        }
    }
}
