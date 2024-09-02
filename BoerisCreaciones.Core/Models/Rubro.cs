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
    public class RubroProductoVM
    {
        public int id_rubroP { get; set; }
        public string nombre { get; set; }

        public RubroProductoVM()
        {

        }

        public RubroProductoVM(int id_rubroP, string nombre)
        {
            this.id_rubroP = id_rubroP;
            this.nombre = nombre;
        }
    }

    public class RubroProductoDTO
    {
        public int id { get; set; }
        public string name { get; set; }

        public RubroProductoDTO()
        {

        }

        public RubroProductoDTO(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}
