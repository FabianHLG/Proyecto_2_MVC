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

        
    }
}
