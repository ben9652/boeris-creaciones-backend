using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoerisCreaciones.Core.Models.Unidades
{
    public class UnidadVM
    {
        public UnidadVM()
        {

        }
        
        public UnidadVM(int id_unidad, string nombre)
        {
            this.id_unidad = id_unidad;
            this.nombre = nombre;
        }

        public int id_unidad { get; set; }
        public string nombre { get; set; }
    }

    public class UnidadDTO
    {
        public UnidadDTO()
        {

        }

        public UnidadDTO(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public int id { get; set; }
        public string name { get; set; }
    }
}
