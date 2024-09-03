using AutoMapper;
using BoerisCreaciones.Core.Models;
using BoerisCreaciones.Repository.Interfaces;
using BoerisCreaciones.Service.Interfaces;

namespace BoerisCreaciones.Service.Services
{
    public class ProvinciasService : IProvinciasService
    {
        private readonly IProvinciasRepository _repository;
        private readonly IMapper _mapper;

        public ProvinciasService(IProvinciasRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public List<ProvinciaDTO> GetAllProvinces()
        {
            List<ProvinciaVM> provinciasBD = _repository.GetAllProvinces();
            List<ProvinciaDTO> provincias = new List<ProvinciaDTO>();
            foreach(ProvinciaVM provinciaBD in provinciasBD)
                provincias.Add(_mapper.Map<ProvinciaDTO>(provinciaBD));

            return provincias;
        }

        public ProvinciaDTO GetProvince(int id)
        {
            ProvinciaVM provinciaBD = _repository.GetProvince(id);
            ProvinciaDTO provincia = _mapper.Map<ProvinciaDTO>(provinciaBD);
            return provincia;
        }

        public ProvinciaDTO CreateProvince(ProvinciaDTO provincia)
        {
            ProvinciaVM provinciaBD = _mapper.Map<ProvinciaVM>(provincia);
            provinciaBD = _repository.CreateProvince(provinciaBD);
            return _mapper.Map<ProvinciaDTO>(provinciaBD);
        }

        public ProvinciaDTO UpdateProvince(ProvinciaDTO provincia, List<string> attributes)
        {
            ProvinciaVM provinciaBD = _mapper.Map<ProvinciaVM>(provincia);
            provinciaBD = _repository.UpdateProvince(provinciaBD, attributes);
            return _mapper.Map<ProvinciaDTO>(provinciaBD);
        }

        public void DeleteProvince(int id)
        {
            _repository.DeleteProvince(id);
        }
    }
}
