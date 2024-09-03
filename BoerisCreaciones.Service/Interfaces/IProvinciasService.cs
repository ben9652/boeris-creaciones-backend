using BoerisCreaciones.Core.Models;

namespace BoerisCreaciones.Service.Interfaces
{
    public interface IProvinciasService
    {
        public List<ProvinciaDTO> GetAllProvinces();
        public ProvinciaDTO GetProvince(int id);
        public ProvinciaDTO CreateProvince(ProvinciaDTO provincia);
        public ProvinciaDTO UpdateProvince(ProvinciaDTO provincia, List<string> attributes);
        public void DeleteProvince(int id);
    }
}
