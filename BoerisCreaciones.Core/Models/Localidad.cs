using BoerisCreaciones.Core.Models.Provincias;

namespace BoerisCreaciones.Core.Models.Localidades
{
    public class LocalidadVM
    {
        public LocalidadVM() { }

        public LocalidadVM(int id_localidad, string nombre, int id_provincia, string provincia)
        {
            this.id_localidad = id_localidad;
            this.nombre = nombre;
            this.id_provincia = id_provincia;
            this.provincia = provincia;
        }

        public int id_localidad { get; set; }
        public string nombre { get; set; }
        public int id_provincia { get; set; }
        public string provincia { get; set; }
    }

    public class LocalidadDTO : LocalidadDTOBase
    {
        public LocalidadDTO() { }
        
        public LocalidadDTO(int id, string name, ProvinciaDTO province) : base(id, name)
        {
            this.province = province;
        }

        public ProvinciaDTO province { get; set; }
    }

    public class LocalidadExpandedDTO : LocalidadDTOBase
    {
        public LocalidadExpandedDTO() { }

        public LocalidadExpandedDTO(int id, string name) : base(id, name)
        {

        }
    }

    public class LocalidadDTOBase
    {
        public LocalidadDTOBase() { }

        public LocalidadDTOBase(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public int id { get; set; }
        public string name { get; set; }
    }
}
