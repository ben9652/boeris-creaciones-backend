using BoerisCreaciones.Core.Models.Unidades;

namespace BoerisCreaciones.Repository.Interfaces
{
    public interface IUnidadesRepository
    {
        public List<UnidadVM> GetUnits();
        public UnidadVM GetUnit(int id);
    }
}
