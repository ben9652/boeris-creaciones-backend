using BoerisCreaciones.Core.Models.Sucursales;

namespace BoerisCreaciones.Service.Interfaces
{
    public interface ICatalogoSucursalesService
    {
        public List<SucursalDTO> GetAll();
        public SucursalDTO GetById(int id);
        public SucursalDTO Create(SucursalDTO sucursal);
        public SucursalDTO Update(SucursalDTO sucursal, List<string> attributes);
        public void Delete(int id);
    }
}
