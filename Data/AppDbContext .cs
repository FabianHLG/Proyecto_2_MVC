using Microsoft.EntityFrameworkCore;
using Proyecto_2_MVC.Models;

namespace Proyecto_2_MVC.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Productos> Productos { get; set; }
        public DbSet<Usuarios> Usuarios { get; set; }
        //public DbSet<Pedidos> Pedidos { get; set; }
    }
}
