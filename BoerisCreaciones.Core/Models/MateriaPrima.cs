using BoerisCreaciones.Core.Models.Rubros;
using BoerisCreaciones.Core.Models.Unidades;

namespace BoerisCreaciones.Core.Models.MateriasPrimas
{
    public class MateriasPrimasItemVM
    {
        public MateriasPrimasItemVM()
        {

        }

        public MateriasPrimasItemVM(int id_matP, int id_rubroMP, string rubro, int id_unidad, string unidad, string nombre, char origen, int cantidad_restante, int cantidad_descartada, string imagen, string comentario)
        {
            this.id_matP = id_matP;
            this.id_rubroMP = id_rubroMP;
            this.rubro = rubro;
            this.id_unidad = id_unidad;
            this.unidad = unidad;
            this.nombre = nombre;
            this.origen = origen;
            this.cantidad_restante = cantidad_restante;
            this.cantidad_descartada = cantidad_descartada;
            this.imagen = imagen;
            this.comentario = comentario;
        }

        public int id_matP { get; set; }
        public int id_rubroMP { get; set; }
        public string rubro { get; set; }
        public int id_unidad { get; set; }
        public string unidad { get; set; }
        public string nombre { get; set; }
        public char origen { get; set; }
        public int cantidad_restante { get; set; }
        public int cantidad_descartada { get; set; }
        public string imagen { get; set; }
        public string? comentario { get; set; }
    }

    public class MateriasPrimasItemDTO
    {
        public MateriasPrimasItemDTO()
        {

        }

        public MateriasPrimasItemDTO(int id, RubroMateriaPrimaDTO category, UnidadDTO unit, string name, char source, int stock, string picture, string comment)
        {
            this.id = id;
            this.category = category;
            this.unit = unit;
            this.name = name;
            this.source = source;
            this.stock = stock;
            this.picture = picture;
            this.comment = comment;
        }

        public int id { get; set; }
        public RubroMateriaPrimaDTO category { get; set; }
        public UnidadDTO unit { get; set; }
        public string name { get; set; }
        public char source { get; set; }
        public int stock { get; set; }
        public string picture { get; set; }
        public string? comment { get; set; }
    }
}
