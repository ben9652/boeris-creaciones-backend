using AutoMapper;
using BoerisCreaciones.Core.Models.Proveedores;
using BoerisCreaciones.Repository.Interfaces;
using BoerisCreaciones.Service.Interfaces;

namespace BoerisCreaciones.Service.Services
{
    public class CatalogoProveedoresService : ICatalogoProveedoresService
    {
        private readonly ICatalogoProveedoresRepository _repository;
        private readonly IMapper _mapper;

        public CatalogoProveedoresService(ICatalogoProveedoresRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public List<ProveedorDTO> GetProviders()
        {
            List<ProveedorDTO> proveedores = new List<ProveedorDTO>();
            List<ProveedorVM> proveedoresBD = _repository.GetProviders();
            foreach(ProveedorVM proveedorBD in proveedoresBD)
                proveedores.Add(_mapper.Map<ProveedorDTO>(proveedorBD));

            return proveedores;
        }

        public ProveedorDTO GetProvider(int id)
        {
            ProveedorVM proveedorBD = _repository.GetProvider(id);
            return _mapper.Map<ProveedorDTO>(proveedorBD);
        }

        public ProveedorDTO CreateProvider(ProveedorDTO provider)
        {
            ProveedorVM proveedorBD = _mapper.Map<ProveedorVM>(provider);
            proveedorBD = _repository.CreateProvider(proveedorBD);
            return _mapper.Map<ProveedorDTO>(proveedorBD);
        }

        public ProveedorDTO UpdateProvider(ProveedorDTO provider, List<string> attributesToChange)
        {
            ProveedorVM proveedorBD = _mapper.Map<ProveedorVM>(provider);
            proveedorBD = _repository.UpdateProvider(proveedorBD, attributesToChange);
            return _mapper.Map<ProveedorDTO>(proveedorBD);
        }

        public void DeleteProvider(int id)
        {
            _repository.DeleteProvider(id);
        }
    }
}
