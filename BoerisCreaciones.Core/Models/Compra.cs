using BoerisCreaciones.Core.Models.MateriasPrimas;
using BoerisCreaciones.Core.Models.Proveedores;
using BoerisCreaciones.Core.Models.Rubros;
using BoerisCreaciones.Core.Models.Socio;

namespace BoerisCreaciones.Core.Models.Compras
{
    public class MateriaPrimaCompraVM
    {
        public MateriaPrimaCompraVM() { }

        public MateriaPrimaCompraVM(int id_matP, int id_rubroMP, string rubro, string nombre, int cantidad, float precio_unitario)
        {
            this.id_matP = id_matP;
            this.id_rubroMP = id_rubroMP;
            this.rubro = rubro;
            this.nombre = nombre;
            this.cantidad = cantidad;
            this.precio_unitario = precio_unitario;
        }

        public int id_matP { get; set; }
        public int id_rubroMP { get; set; }
        public string rubro { get; set; }
        public string nombre { get; set; }
        public int cantidad { get; set; }
        public float precio_unitario { get; set; }
    }

    public class MateriaPrimaCompraDTO
    {
        public MateriaPrimaCompraDTO() { }

        public MateriaPrimaCompraDTO(int raw_material_id, RubroMateriaPrimaDTO category, string name, int quantity, float unit_price)
        {
            this.raw_material_id = raw_material_id;
            this.category = category;
            this.name = name;
            this.quantity = quantity;
            this.unit_price = unit_price;
        }

        public int raw_material_id { get; set; }
        public RubroMateriaPrimaDTO category { get; set; }
        public string name { get; set; }
        public int quantity { get; set; }
        public float unit_price { get; set; }
    }

    public class NuevaCompraDTO
    {
        public NuevaCompraDTO()
        {
        }

        public NuevaCompraDTO(List<MateriaPrimaCompraDTO> raw_materials, ProveedorDTO provider, SocioDTO partner, string currency, char payment_type, char reception_mode, string description)
        {
            this.raw_materials = raw_materials;
            this.provider = provider;
            this.partner = partner;
            this.currency = currency;
            this.payment_type = payment_type;
            this.reception_mode = reception_mode;
            this.description = description;
        }

        public List<MateriaPrimaCompraDTO> raw_materials { get; set; }
        public ProveedorDTO provider { get; set; }
        public SocioDTO partner { get; set; }
        public string currency { get; set; }
        public char payment_type { get; set; }
        public char reception_mode { get; set; }
        public string description { get; set; }
    }

    public class CompraVM
    {
        public CompraVM() { }

        public CompraVM(int id_compra, UsuarioVM socio_que_hizo_pedido, ProveedorVM proveedor, string descripcion, DateTime fecha_pedido, DateTime? fecha_recepcion, DateTime? fecha_cancelado, string moneda, char tipo_pago, char modo_recepcion, char estado, float? precio, string? factura)
        {
            this.id_compra = id_compra;
            this.socio_que_hizo_pedido = socio_que_hizo_pedido;
            this.proveedor = proveedor;
            this.descripcion = descripcion;
            this.fecha_pedido = fecha_pedido;
            this.fecha_recepcion = fecha_recepcion;
            this.fecha_cancelado = fecha_cancelado;
            this.moneda = moneda;
            this.tipo_pago = tipo_pago;
            this.modo_recepcion = modo_recepcion;
            this.estado = estado;
            this.precio = precio;
            this.factura = factura;
        }

        public int id_compra { get; set; }
        public UsuarioVM socio_que_hizo_pedido { get; set; }
        public ProveedorVM proveedor { get; set; }
        public string descripcion { get; set; }
        public DateTime fecha_pedido { get; set; }
        public DateTime? fecha_recepcion { get; set; }
        public DateTime? fecha_cancelado { get; set; }
        public string moneda { get; set; }
        public char tipo_pago { get; set; }
        public char modo_recepcion { get; set; }
        public char estado { get; set; }
        public float? precio { get; set; }
        public string? factura { get; set; }
    }

    public class CompraDTO
    {
        public CompraDTO()
        {
            raw_materials = new List<MateriaPrimaCompraDTO>();
        }

        public CompraDTO(int id, UsuarioDTO requester_partner, ProveedorDTO provider, string description, DateTime order_date, DateTime? reception_date, DateTime? canceled_date, string currency, char payment_type, char reception_mode, char status, float? price, string? invoice)
        {
            this.id= id;
            this.requester_partner = requester_partner;
            this.provider = provider;
            this.description = description;
            this.order_date = order_date;
            this.reception_date = reception_date;
            this.canceled_date = canceled_date;
            this.currency = currency;
            this.payment_type = payment_type;
            this.reception_mode = reception_mode;
            this.status = status;
            this.price = price;
            this.invoice = invoice;

            raw_materials = new List<MateriaPrimaCompraDTO>();
        }

        public int id { get; set; }
        public UsuarioDTO requester_partner { get; set; }
        public ProveedorDTO provider { get; set; }
        public List<MateriaPrimaCompraDTO> raw_materials { get; set; }
        public string description { get; set; }
        public DateTime order_date { get; set; }
        public DateTime? reception_date { get; set; }
        public DateTime? canceled_date { get; set; }
        public string currency { get; set; }
        public char payment_type { get; set; }
        public char reception_mode { get; set; }
        public char status { get; set; }
        public float? price { get; set; }
        public string? invoice { get; set; }
    }
}
