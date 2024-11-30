namespace Proyecto_2_MVC.Models
{
    public class Carrito
    {
        // Lista estática para simular el carrito
        public static List<Carrito> carrito = new List<Carrito>();

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public string Categoria { get; set; }
        public int Stock { get; set; }

        public string IconoFontAwesome { get; set; }

        public int Cantidad { get; set; }

    }
}
