using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoerisCreaciones.Core.Models
{
    namespace MateriasPrimas
    {
        public class MateriaPrimaDTO
        {
            private uint id_materiaPrima;
            private uint id_compra;
            private char estado;
            private string nombre;
            private float precio;
            private string proveedor;
            private DateTime fecha_orden;
            private DateTime? fecha_stockeo;
            private DateTime? fecha_finalizacion_uso;

            // Solo presentes en materias primas no contables
            private float? medida;
#nullable enable
            private string? unidad_medida;

            // Solo presente en materias primas contables
            private uint? cantidad_restante;
            private uint? cantidad_por_paquete;

            private string? comentario;
            private List<UsoMateriaPrima> usos;

            /* Constructores para las materias primas CONTABLES */

            /// <summary>
            /// Materia prima contable NUEVA
            /// </summary>
            /// <param name="id_materiaPrima"></param>
            /// <param name="id_compra"></param>
            /// <param name="nombre"></param>
            /// <param name="precio"></param>
            /// <param name="proveedor"></param>
            /// <param name="fecha_orden"></param>
            /// <param name="cantidad_por_paquete"></param>
            /// <param name="comentario"></param>
            public MateriaPrimaDTO(
                uint id_materiaPrima,
                uint id_compra,
                string nombre,
                float precio,
                string proveedor,
                DateTime fecha_orden,
                uint cantidad_por_paquete,
                string? comentario
                )
            {
                Estado = 'P';
                this.id_materiaPrima = id_materiaPrima;
                this.id_compra = id_compra;
                this.nombre = nombre;
                this.precio = precio;
                this.proveedor = proveedor;
                this.fecha_orden = fecha_orden;
                this.cantidad_por_paquete = cantidad_por_paquete;
                this.comentario = comentario;
                usos = new List<UsoMateriaPrima>();
            }

            /// <summary>
            /// Materia prima contable EN STOCK
            /// </summary>
            /// <param name="id_materiaPrima"></param>
            /// <param name="id_compra"></param>
            /// <param name="nombre"></param>
            /// <param name="precio"></param>
            /// <param name="proveedor"></param>
            /// <param name="fecha_orden"></param>
            /// <param name="fecha_stockeo"></param>
            /// <param name="cantidad_por_paquete"></param>
            /// <param name="cantidad_restante"></param>
            public MateriaPrimaDTO(
                uint id_materiaPrima,
                uint id_compra,
                string nombre,
                float precio,
                string proveedor,
                DateTime fecha_orden,
                DateTime fecha_stockeo,
                uint cantidad_por_paquete,
                uint cantidad_restante
                )
            {
                Estado = 'A';
                this.id_materiaPrima = id_materiaPrima;
                this.id_compra = id_compra;
                this.nombre = nombre;
                this.precio = precio;
                this.proveedor = proveedor;
                this.fecha_orden = fecha_orden;
                this.fecha_stockeo = fecha_stockeo;
                this.cantidad_por_paquete = cantidad_por_paquete;
                this.cantidad_restante = cantidad_restante;
                usos = new List<UsoMateriaPrima>();
            }

            /// <summary>
            /// Materia prima contable UTILIZADA
            /// </summary>
            /// <param name="id_materiaPrima"></param>
            /// <param name="id_compra"></param>
            /// <param name="nombre"></param>
            /// <param name="precio"></param>
            /// <param name="proveedor"></param>
            /// <param name="fecha_orden"></param>
            /// <param name="fecha_stockeo"></param>
            /// <param name="fecha_finalizacion_uso"></param>
            /// <param name="cantidad_por_paquete"></param>
            /// <param name="cantidad_restante"></param>
            public MateriaPrimaDTO(
                uint id_materiaPrima,
                uint id_compra,
                string nombre,
                float precio,
                string proveedor,
                DateTime fecha_orden,
                DateTime fecha_stockeo,
                DateTime fecha_finalizacion_uso,
                uint cantidad_por_paquete,
                uint cantidad_restante
                )
            {
                Estado = 'U';
                this.id_materiaPrima = id_materiaPrima;
                this.id_compra = id_compra;
                this.nombre = nombre;
                this.precio = precio;
                this.proveedor = proveedor;
                this.fecha_orden = fecha_orden;
                this.fecha_stockeo = fecha_stockeo;
                this.fecha_finalizacion_uso = fecha_finalizacion_uso;
                this.cantidad_por_paquete = cantidad_por_paquete;
                this.cantidad_restante = cantidad_restante;
                usos = new List<UsoMateriaPrima>();
            }

            /* Constructores para las materias primas NO CONTABLES */

            /// <summary>
            /// Materia prima NO contable NUEVA
            /// </summary>
            /// <param name="id_materiaPrima"></param>
            /// <param name="id_compra"></param>
            /// <param name="nombre"></param>
            /// <param name="precio"></param>
            /// <param name="medida"></param>
            /// <param name="unidad_medida"></param>
            /// <param name="proveedor"></param>
            /// <param name="fecha_orden"></param>
            /// <param name="comentario"></param>
            public MateriaPrimaDTO(
                uint id_materiaPrima,
                uint id_compra,
                string nombre,
                float precio,
                float medida,
                string unidad_medida,
                string proveedor,
                DateTime fecha_orden,
                string? comentario
                )
            {
                Estado = 'P';
                this.id_materiaPrima = id_materiaPrima;
                this.id_compra = id_compra;
                this.nombre = nombre;
                this.precio = precio;
                this.medida = medida;
                this.unidad_medida = unidad_medida;
                this.proveedor = proveedor;
                this.fecha_orden = fecha_orden;
                this.comentario = comentario;
                usos = new List<UsoMateriaPrima>();
            }

            /// <summary>
            /// Materia prima NO contable EN STOCK
            /// </summary>
            /// <param name="id_materiaPrima"></param>
            /// <param name="id_compra"></param>
            /// <param name="nombre"></param>
            /// <param name="precio"></param>
            /// <param name="medida"></param>
            /// <param name="unidad_medida"></param>
            /// <param name="proveedor"></param>
            /// <param name="fecha_orden"></param>
            /// <param name="fecha_stockeo"></param>
            public MateriaPrimaDTO(
                uint id_materiaPrima,
                uint id_compra,
                string nombre,
                float precio,
                float medida,
                string unidad_medida,
                string proveedor,
                DateTime fecha_orden,
                DateTime fecha_stockeo
                )
            {
                Estado = 'A';
                this.id_materiaPrima = id_materiaPrima;
                this.id_compra = id_compra;
                this.nombre = nombre;
                this.precio = precio;
                this.medida = medida;
                this.unidad_medida = unidad_medida;
                this.proveedor = proveedor;
                this.fecha_orden = fecha_orden;
                this.fecha_stockeo = fecha_stockeo;
                usos = new List<UsoMateriaPrima>();
            }

            /// <summary>
            /// Materia prima NO contable UTILIZADA
            /// </summary>
            /// <param name="id_materiaPrima"></param>
            /// <param name="id_compra"></param>
            /// <param name="nombre"></param>
            /// <param name="precio"></param>
            /// <param name="medida"></param>
            /// <param name="unidad_medida"></param>
            /// <param name="proveedor"></param>
            /// <param name="fecha_orden"></param>
            /// <param name="fecha_stockeo"></param>
            /// <param name="fecha_finalizacion_uso"></param>
            public MateriaPrimaDTO(
                uint id_materiaPrima,
                uint id_compra,
                string nombre,
                float precio,
                float medida,
                string unidad_medida,
                string proveedor,
                DateTime fecha_orden,
                DateTime fecha_stockeo,
                DateTime fecha_finalizacion_uso
                )
            {
                Estado = 'U';
                this.id_materiaPrima = id_materiaPrima;
                this.id_compra = id_compra;
                this.nombre = nombre;
                this.precio = precio;
                this.medida = medida;
                this.unidad_medida = unidad_medida;
                this.proveedor = proveedor;
                this.fecha_orden = fecha_orden;
                this.fecha_stockeo = fecha_stockeo;
                this.fecha_finalizacion_uso = fecha_finalizacion_uso;
                usos = new List<UsoMateriaPrima>();
            }

            public uint Id_materiaPrima { get => id_materiaPrima; set => id_materiaPrima = value; }
            public uint Id_compra { get => id_compra; set => id_compra = value; }
            public char Estado {
                get => estado;
                set {
                    if (value != 'P' && value != 'A' && value != 'U')
                        throw new ArgumentException("El estado solo puede tomar los valores \'P\', \'A\', o \'U\'");

                    estado = value;
                }
            }
            public string Nombre { get => nombre; set => nombre = value; }
            public float Precio { get => precio; set => precio = value; }
            public string Proveedor { get => proveedor; set => proveedor = value; }
            public DateTime Fecha_orden { get => fecha_orden; set => fecha_orden = value; }
            public DateTime? Fecha_stockeo { get => fecha_stockeo; set => fecha_stockeo = value; }
            public DateTime? Fecha_finalizacion_uso { get => fecha_finalizacion_uso; set => fecha_finalizacion_uso = value; }
            public float? Medida { get => medida; set => medida = value; }
            public string? Unidad_medida { get => unidad_medida; set => unidad_medida = value; }
            public uint? Cantidad_restante { get => cantidad_restante; set => cantidad_restante = value; }
            public uint? Cantidad_por_paquete { get => cantidad_por_paquete; set => cantidad_por_paquete = value; }
            public string? Comentario { get => comentario; set => comentario = value; }
            public List<UsoMateriaPrima> Usos { get => usos; set => usos = value; }
        }

        public class UsoMateriaPrima
        {
            private DateTime fecha_uso;
            private MateriaPrimaDTO? materia_prima;
            private uint cantidad;
            private string? nombre;
            private string? apellido;

            /// <summary>
            /// Constructor para usos sobre materias primas NO CONTABLES
            /// </summary>
            /// <param name="fecha_uso"></param>
            /// <param name="materia_prima"></param>
            /// <param name="cantidad"></param>
            /// <param name="nombre"></param>
            /// <param name="apellido"></param>
            public UsoMateriaPrima(
                DateTime fecha_uso,
                MateriaPrimaDTO? materia_prima,
                uint cantidad,
                string? nombre,
                string? apellido
                )
            {
                this.fecha_uso = fecha_uso;
                this.materia_prima = materia_prima;
                this.cantidad = cantidad;
                this.nombre = nombre;
                this.apellido = apellido;
            }

            /// <summary>
            /// Constructor para usos sobre materias primas CONTABLES
            /// </summary>
            /// <param name="fecha_uso"></param>
            /// <param name="cantidad"></param>
            /// <param name="nombre"></param>
            /// <param name="apellido"></param>
            public UsoMateriaPrima(
                DateTime fecha_uso,
                uint cantidad,
                string nombre,
                string apellido
                )
            {
                this.fecha_uso = fecha_uso;
                this.cantidad = cantidad;
                this.nombre = nombre;
                this.apellido = apellido;
            }

            public DateTime Fecha_uso { get => fecha_uso; set => fecha_uso = value; }
            public MateriaPrimaDTO? Materia_Prima { get => materia_prima; set => materia_prima = value; }
            public uint Cantidad { get => cantidad; set => cantidad = value; }
            public string? Nombre { get => nombre; set => nombre = value; }
            public string? Apellido { get => apellido; set => apellido = value; }
        }
    }
}
