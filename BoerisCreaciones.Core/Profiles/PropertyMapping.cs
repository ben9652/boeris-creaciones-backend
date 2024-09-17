using BoerisCreaciones.Core.Helpers;
using System.Reflection;

namespace BoerisCreaciones.Core.Profiles
{
    public class PropertyMapping
    {
        public static List<PatchUpdate> MapPatchProperties(object origen, object destino, List<PatchUpdate> propiedadesOrigen)
        {
            Type tipoOrigen = origen.GetType();
            Type tipoDestino = destino.GetType();

            PropertyInfo[] propiedadesDestino = tipoDestino.GetProperties();

            List<PatchUpdate> patchDest = new();

            for (int i = 0; i < propiedadesOrigen.Count; i++)
            {
                // Obtener PropertyInfo de la propiedad en el objeto origen
                PropertyInfo propiedadOrigen = tipoOrigen.GetProperty(propiedadesOrigen[i].path);
                // Obtener PropertyInfo de la propiedad correspondiente en el objeto destino
                PropertyInfo propiedadDestino = tipoDestino.GetProperty(propiedadesDestino[i].Name);

                if (propiedadOrigen != null && propiedadDestino != null && propiedadDestino.CanWrite)
                {
                    // Obtener valor de la propiedad origen
                    var valor = propiedadOrigen.GetValue(origen);
                    // Asignar valor a la propiedad destino
                    propiedadDestino.SetValue(destino, valor);
                }
            }

            return patchDest;
        }
    }
}
