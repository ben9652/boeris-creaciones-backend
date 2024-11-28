using AutoMapper;
using BoerisCreaciones.Core.Models.Localidades;
using BoerisCreaciones.Core.Models.PrimeNG.Dropdown;
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

        public List<SelectItemGroup<LocalidadDTOBase, SucursalDTOBase>> GetGroupedDropdown()
        {
            List<SelectItemGroup<LocalidadDTOBase, SucursalDTOBase>> groupedDropdown = new();

            List<SucursalVM> sucursalesBD = _repository.GetAll();

            sucursalesBD = sucursalesBD.OrderBy(sucursal => sucursal.id_localidad).ToList();

            LocalidadDTOBase localidad = new LocalidadDTOBase(sucursalesBD[0].id_localidad, sucursalesBD[0].localidad);
            List<SelectItem<SucursalDTOBase>> group = new();
            foreach(SucursalVM sucursal in sucursalesBD)
            {
                if(sucursal.id_localidad != localidad.id)
                {
                    List<SelectItem<SucursalDTOBase>> newGroup = new(group);
                    groupedDropdown.Add(new SelectItemGroup<LocalidadDTOBase, SucursalDTOBase>(localidad.name, localidad, newGroup));
                    localidad = new LocalidadDTOBase(sucursal.id_localidad, sucursal.localidad);
                    group.Clear();
                }

                SucursalDTOBase sucursalDTO = _mapper.Map<SucursalDTOBase>(sucursal);
                group.Add(new SelectItem<SucursalDTOBase>(sucursalDTO.name, sucursalDTO));
            }

            groupedDropdown.Add(new SelectItemGroup<LocalidadDTOBase, SucursalDTOBase>(localidad.name, localidad, group));

            return groupedDropdown;
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
