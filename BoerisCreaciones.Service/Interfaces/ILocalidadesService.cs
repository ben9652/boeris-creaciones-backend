using BoerisCreaciones.Core.Models.Localidades;

namespace BoerisCreaciones.Service.Interfaces
{
    public interface ILocalidadesService
    {
        public List<LocalidadDTO> GetAll();
        public List<LocalidadExpandedDTO> GetAllLocalitiesWithBranches();
        public LocalidadDTO GetById(int id);
        public LocalidadExpandedDTO GetLocalityWithBranches(int id);
        public LocalidadDTO Create(LocalidadDTO localidad);
        public LocalidadDTO Update(LocalidadDTO localidad, List<string> attributes);
        public void Delete(int id);
    }
}
