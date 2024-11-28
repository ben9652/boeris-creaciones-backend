using BoerisCreaciones.Core.Models.PrimeNG.Dropdown;
using BoerisCreaciones.Core.Models.Proveedores;
using BoerisCreaciones.Core.Models.Rubros;

namespace BoerisCreaciones.Service.Interfaces
{
    public interface ICatalogoProveedoresService
    {
        List<ProveedorDTO> GetProviders();
        public ProveedorDTO GetProvider(int id);
        public List<SelectItemGroup<RubroMateriaPrimaDTO, ProveedorDTOBase>> GetGroupedDropdown();
        public ProveedorDTO CreateProvider(ProveedorDTO provider);
        public ProveedorDTO UpdateProvider(ProveedorDTO provider, List<string> attributesToChange);
        public void DeleteProvider(int id);
    }
}
