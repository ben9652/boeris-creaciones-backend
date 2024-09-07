using BoerisCreaciones.Core.Models.Localidades;

namespace BoerisCreaciones.Repository.Interfaces
{
    public interface ILocalidadesRepository
    {
        public List<LocalidadVM> GetAll();
        public LocalidadVM GetById(int id);
        public List<LocalidadVM> GetByIdProvince(int id);
        public LocalidadVM Create(LocalidadVM localidad);
        public LocalidadVM Update(LocalidadVM localidad, List<string> attributes);
        public void Delete(int id);
    }
}
