namespace BoerisCreaciones.Core.Models.Rubros
{
    public class RubroMateriaPrimaVM
    {
        public int id_rubroMP { get; set; }
        public string nombre { get; set; }

        public RubroMateriaPrimaVM()
        {

        }

        public RubroMateriaPrimaVM(int id_rubroMP, string nombre)
        {
            this.id_rubroMP = id_rubroMP;
            this.nombre = nombre;
        }
    }

    public class RubroMateriaPrimaDTO
    {
        public int id { get; set; }
        public string name { get; set; }

        public RubroMateriaPrimaDTO()
        {

        }

        public RubroMateriaPrimaDTO(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}
