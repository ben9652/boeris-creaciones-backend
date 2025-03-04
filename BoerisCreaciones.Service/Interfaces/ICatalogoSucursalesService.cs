using BoerisCreaciones.Core.Models.Localidades;
using BoerisCreaciones.Core.Models.PrimeNG.Dropdown;
using BoerisCreaciones.Core.Models.Sucursales;

namespace BoerisCreaciones.Service.Interfaces
{
    public interface ICatalogoSucursalesService
    {
        public List<SucursalDTO> GetAll();
        public SucursalDTO GetById(int id);
        public List<SelectItemGroup<LocalidadDTOBase, SucursalDTO>> GetGroupedDropdown();
        public SucursalDTO Create(SucursalDTO sucursal);
        public SucursalDTO Update(SucursalDTO sucursal, List<string> attributes);
        public void Delete(int id);
    }
}
