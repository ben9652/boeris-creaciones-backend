using BoerisCreaciones.Core.Models.Localidades;
using System.Collections;

namespace BoerisCreaciones.Core.Models.Provincias
{
    public class ProvinciaVM
    {
        public ProvinciaVM()
        {

        }

        public ProvinciaVM(int id_provincia, string nombre)
        {
            this.id_provincia = id_provincia;
            this.nombre = nombre;
        }

        public int id_provincia { get; set; }
        public string nombre { get; set; }
    }

    public class ProvinciaDTO
    {
        public ProvinciaDTO()
        {

        }

        public ProvinciaDTO(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public int id { get; set; }
        public string name { get; set; }
    }

    public class ProvinciaExpandedDTO : ProvinciaDTO
    {
        public ProvinciaExpandedDTO(int id, string name) : base(id, name)
        {
            localities = new List<LocalidadExpandedDTO>();
        }

        public ProvinciaExpandedDTO()
        {
            localities = new List<LocalidadExpandedDTO>();
        }

        public List<LocalidadExpandedDTO> localities { get; set; }
    }
}
