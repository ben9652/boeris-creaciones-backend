using BoerisCreaciones.Core.Models.Provincias;

namespace BoerisCreaciones.Repository.Interfaces
{
    public interface IProvinciasRepository
    {
        public List<ProvinciaVM> GetAllProvinces();
        public ProvinciaVM GetProvince(int id);
        public ProvinciaVM CreateProvince(ProvinciaVM provincia);
        public ProvinciaVM UpdateProvince(ProvinciaVM provincia, List<string> attributesToChange);
        public void DeleteProvince(int id);
    }
}
