using BoerisCreaciones.Core.Models.Rubros;

namespace BoerisCreaciones.Core.Models.Proveedores
{
    public class ProveedorVM
    {
        public ProveedorVM()
        {

        }

        public ProveedorVM(int id, string nombre, int id_rubro, string rubroAsociado, string? domicilio, long? telefono, string? cvu, string? alias)
        {
            this.id = id;
            this.nombre = nombre;
            this.id_rubro = id_rubro;
            this.rubroAsociado = rubroAsociado;
            this.domicilio = domicilio;
            this.telefono = telefono;
            this.cvu = cvu;
            this.alias = alias;
        }

        public int id { get; set; }
        public string nombre { get; set; }
        public int id_rubro { get; set; }
        public string rubroAsociado { get; set; }
        public string? domicilio { get; set; }
        public long? telefono { get; set; }
        public string? cvu { get; set; }
        public string? alias { get; set; }
    }

    public class ProveedorDTOBase
    {
        public ProveedorDTOBase()
        {

        }

        public ProveedorDTOBase(int id, string name, string? residence, long? phone, string? cvu_or_alias)
        {
            this.id = id;
            this.name = name;
            this.residence = residence;
            this.phone = phone;
            this.cvu_or_alias = cvu_or_alias;
        }

        public int id { get; set; }
        public string name { get; set; }
        public string? residence { get; set; }
        public long? phone { get; set; }
        public string? cvu_or_alias { get; set; }
    }

    public class ProveedorDTO : ProveedorDTOBase
    {
        public ProveedorDTO()
        {

        }

        public ProveedorDTO(int id, string name, RubroMateriaPrimaDTO category, string? residence, long? phone, string? cvu_or_alias)
            : base(id, name, residence, phone, cvu_or_alias)
        {
            this.category = category;
        }

        public RubroMateriaPrimaDTO category { get; set; }
    }
}
