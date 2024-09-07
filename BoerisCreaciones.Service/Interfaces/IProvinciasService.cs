using BoerisCreaciones.Core.Models.Provincias;

namespace BoerisCreaciones.Service.Interfaces
{
    public interface IProvinciasService
    {
        public List<ProvinciaDTO> GetAllProvinces();
        public List<ProvinciaExpandedDTO> GetAllProvincesWithLocalities();
        public ProvinciaDTO GetProvince(int id);
        public ProvinciaExpandedDTO GetProvinceWithLocalities(int id);
        public ProvinciaDTO CreateProvince(ProvinciaDTO provincia);
        public ProvinciaDTO UpdateProvince(ProvinciaDTO provincia, List<string> attributes);
        public void DeleteProvince(int id);
    }
}
