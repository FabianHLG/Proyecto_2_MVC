namespace Proyecto_2_MVC.Models
{
    public class Pedidos
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdProducto { get; set; }
        public int CantCompra { get; set; }
        public decimal PrecioTotal { get; set; }
        public string Estado { get; set; }
        public DateTime Fecha { get; set; }
        public string NumeroPedido { get; set; }
    }
}
