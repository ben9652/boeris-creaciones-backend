using BoerisCreaciones.Core.Models.Localidades;

namespace BoerisCreaciones.Core.Models.Sucursales
{
    public class SucursalVM
    {
        public SucursalVM() { }

        public SucursalVM(int id_sucursal, string nombre, string domicilio, int id_localidad, string localidad)
        {
            this.id_sucursal = id_sucursal;
            this.nombre = nombre;
            this.domicilio = domicilio;
            this.id_localidad = id_localidad;
            this.localidad = localidad;
        }

        public int id_sucursal { get; set; }
        public string nombre { get; set; }
        public string domicilio { get; set; }
        public int id_localidad { get; set; }
        public string localidad { get; set; }
    }

    public class SucursalDTO : SucursalDTOBase
    {
        public SucursalDTO() { }

        public SucursalDTO(int id, string name, string domicile, LocalidadDTO locality) : base(id, name, domicile)
        {
            this.locality = locality;
        }

        public LocalidadDTO locality { get; set; }
    }

    public class SucursalExpandedDTO : SucursalDTOBase
    {
        public SucursalExpandedDTO() { }

        public SucursalExpandedDTO(int id, string name, string domicile) : base(id, name, domicile)
        {

        }
    }

    public class SucursalDTOBase
    {
        public SucursalDTOBase() { }

        public SucursalDTOBase(int id, string name, string domicile)
        {
            this.id = id;
            this.name = name;
            this.domicile = domicile;
        }

        public int id { get; set; }
        public string name { get; set; }
        public string domicile { get; set; }
    }
}
