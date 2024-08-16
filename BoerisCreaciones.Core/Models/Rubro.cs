namespace BoerisCreaciones.Core.Models.Rubros
{
    public class Rubro
    {
        public Rubro(string nombre)
        {
            this.nombre = nombre;
        }

        public string nombre { get; set; }
    }

    public class RubroMateriaPrima : Rubro
    {
        public int id_rubroMP { get; set; }

        public RubroMateriaPrima(int id_rubro, string nombre) : base(nombre)
        {
            id_rubroMP = id_rubro;
        }
    }
}
