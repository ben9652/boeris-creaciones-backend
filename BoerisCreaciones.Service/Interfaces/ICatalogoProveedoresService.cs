using BoerisCreaciones.Core.Models.Proveedores;

namespace BoerisCreaciones.Service.Interfaces
{
    public interface ICatalogoProveedoresService
    {
        List<ProveedorDTO> GetProviders();
        public ProveedorDTO GetProvider(int id);
        public ProveedorDTO CreateProvider(ProveedorDTO provider);
        public ProveedorDTO UpdateProvider(ProveedorDTO provider, List<string> attributesToChange);
        public void DeleteProvider(int id);
    }
}
