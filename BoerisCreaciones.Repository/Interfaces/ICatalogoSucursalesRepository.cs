using BoerisCreaciones.Core.Models.Sucursales;

namespace BoerisCreaciones.Repository.Interfaces
{
    public interface ICatalogoSucursalesRepository
    {
        public List<SucursalVM> GetAll();
        public SucursalVM GetById(int id);
        public List<SucursalVM> GetByIdLocality(int id);
        public SucursalVM Create(SucursalVM sucursal);
        public SucursalVM Update(SucursalVM sucursal, List<string> attributes);
        public void Delete(int id);
    }
}
