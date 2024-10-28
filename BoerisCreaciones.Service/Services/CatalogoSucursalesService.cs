using AutoMapper;
using BoerisCreaciones.Core.Models.Sucursales;
using BoerisCreaciones.Repository.Interfaces;
using BoerisCreaciones.Service.Interfaces;

namespace BoerisCreaciones.Service.Services
{
    public class CatalogoSucursalesService : ICatalogoSucursalesService
    {
        private readonly ICatalogoSucursalesRepository _repository;
        private readonly ILocalidadesService _localidadesService;
        private readonly IMapper _mapper;

        public CatalogoSucursalesService(ICatalogoSucursalesRepository repository, IMapper mapper, ILocalidadesService localidadesService)
        {
            _repository = repository;
            _mapper = mapper;
            _localidadesService = localidadesService;
        }

        public List<SucursalDTO> GetAll()
        {
            List<SucursalVM> sucursalesBD = _repository.GetAll();
            List<SucursalDTO> sucursales = new List<SucursalDTO>();
            foreach(SucursalVM sucursalBD in sucursalesBD)
            {
                SucursalDTO sucursalDTO = _mapper.Map<SucursalDTO>(sucursalBD);
                sucursalDTO.locality = _localidadesService.GetById(sucursalDTO.locality.id);
                sucursales.Add(sucursalDTO);
            }

            return sucursales;
        }

        public SucursalDTO GetById(int id)
        {
            SucursalVM sucursalBD = _repository.GetById(id);

            SucursalDTO sucursalDTO = _mapper.Map<SucursalDTO>(sucursalBD);
            sucursalDTO.locality = _localidadesService.GetById(sucursalDTO.locality.id);

            return sucursalDTO;
        }

        public SucursalDTO Create(SucursalDTO sucursal)
        {
            SucursalVM sucursalBD = _mapper.Map<SucursalVM>(sucursal);
            sucursalBD = _repository.Create(sucursalBD);

            SucursalDTO sucursalDTO = _mapper.Map<SucursalDTO>(sucursalBD);
            sucursalDTO.locality = _localidadesService.GetById(sucursalDTO.locality.id);

            return sucursalDTO;
        }

        public SucursalDTO Update(SucursalDTO sucursal, List<string> attributes)
        {
            SucursalVM sucursalBD = _mapper.Map<SucursalVM>(sucursal);
            sucursalBD = _repository.Update(sucursalBD, attributes);
            
            SucursalDTO sucursalDTO = _mapper.Map<SucursalDTO>(sucursalBD);
            sucursalDTO.locality = _localidadesService.GetById(sucursalDTO.locality.id);

            return sucursalDTO;
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }
    }
}
