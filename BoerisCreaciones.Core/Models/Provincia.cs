namespace BoerisCreaciones.Core.Models
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
}
