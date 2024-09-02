using BoerisCreaciones.Core.Models.Proveedores;

namespace BoerisCreaciones.Repository.Interfaces
{
    public interface ICatalogoProveedoresRepository
    {
        public List<ProveedorVM> GetProviders();
        public ProveedorVM GetProvider(int id);
        public ProveedorVM CreateProvider(ProveedorVM provider);
        public ProveedorVM UpdateProvider(ProveedorVM provider, List<string> attributesToChange);
        public void DeleteProvider(int id);
    }
}
