using BoerisCreaciones.Core.Models.Rubros;

namespace BoerisCreaciones.Core.Models.Productos
{
    public class ProductoVM
    {
        public ProductoVM()
        {

        }

        public ProductoVM(int id_producto, int id_rubroP, string rubro, string nombre, float precio, int cantidad_restante, int cantidad_descartada, string imagen, string comentario)
        {
            this.id_producto = id_producto;
            this.id_rubroP = id_rubroP;
            this.rubro = rubro;
            this.nombre = nombre;
            this.precio = precio;
            this.cantidad_restante = cantidad_restante;
            this.cantidad_descartada = cantidad_descartada;
            this.imagen = imagen;
            this.comentario = comentario;
        }

        public int id_producto { get; set; }
        public int id_rubroP { get; set; }
        public string rubro { get; set; }
        public string nombre { get; set; }
        public float precio { get; set; }
        public int cantidad_restante { get; set; }
        public int cantidad_descartada { get; set; }
        public string imagen { get; set; }
        public string? comentario { get; set; }
    }

    public class ProductoDTO
    {
        public ProductoDTO()
        {

        }

        public ProductoDTO(int id, RubroProductoDTO category, string name, float price, int stock, string picture, string comment)
        {
            this.id = id;
            this.category = category;
            this.name = name;
            this.price = price;
            this.stock = stock;
            this.picture = picture;
            this.comment = comment;
        }

        public int id { get; set; }
        public RubroProductoDTO category { get; set; }
        public string name { get; set; }
        public float price { get; set; }
        public int stock { get; set; }
        public string picture { get; set; }
        public string? comment { get; set; }
    }
}
