using BoerisCreaciones.Core.Models.Unidades;

namespace BoerisCreaciones.Service.Interfaces
{
    public interface IUnidadesService
    {
        public List<UnidadDTO> GetUnits();
        public UnidadDTO GetUnit(int id);
    }
}
