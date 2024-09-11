using AutoMapper;
using BoerisCreaciones.Core.Models.Sucursales;
using BoerisCreaciones.Repository.Interfaces;
using BoerisCreaciones.Service.Interfaces;

namespace BoerisCreaciones.Service.Services
{
    public class CatalogoSucursalesService : ICatalogoSucursalesService
    {
        private readonly ICatalogoSucursalesRepository _repository;
        private readonly IMapper _mapper;

        public CatalogoSucursalesService(ICatalogoSucursalesRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public List<SucursalDTO> GetAll()
        {
            List<SucursalVM> sucursalesBD = _repository.GetAll();
            List<SucursalDTO> sucursales = new List<SucursalDTO>();
            foreach(SucursalVM sucursalBD in sucursalesBD)
                sucursales.Add(_mapper.Map<SucursalDTO>(sucursalBD));

            return sucursales;
        }

        public SucursalDTO GetById(int id)
        {
            SucursalVM sucursalBD = _repository.GetById(id);
            SucursalDTO sucursal = _mapper.Map<SucursalDTO>(sucursalBD);
            return sucursal;
        }

        public SucursalDTO Create(SucursalDTO sucursal)
        {
            SucursalVM sucursalBD = _mapper.Map<SucursalVM>(sucursal);
            sucursalBD = _repository.Create(sucursalBD);
            return _mapper.Map<SucursalDTO>(sucursalBD);
        }

        public SucursalDTO Update(SucursalDTO sucursal, List<string> attributes)
        {
            SucursalVM sucursalBD = _mapper.Map<SucursalVM>(sucursal);
            sucursalBD = _repository.Update(sucursalBD, attributes);
            return _mapper.Map<SucursalDTO>(sucursalBD);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }
    }
}
