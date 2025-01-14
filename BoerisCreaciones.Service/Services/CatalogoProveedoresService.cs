using AutoMapper;
using BoerisCreaciones.Core.Models.PrimeNG.Dropdown;
using BoerisCreaciones.Core.Models.Proveedores;
using BoerisCreaciones.Core.Models.Rubros;
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

        public List<SelectItemGroup<RubroMateriaPrimaDTO, ProveedorDTOBase>> GetGroupedDropdown()
        {
            List<SelectItemGroup<RubroMateriaPrimaDTO, ProveedorDTOBase>> groupedDropdown = new();

            List<ProveedorVM> proveedoresBD = _repository.GetProviders();

            if(proveedoresBD.Count == 0)
                return groupedDropdown;

            proveedoresBD = proveedoresBD.OrderBy(proveedor => proveedor.id_rubro).ToList();

            RubroMateriaPrimaDTO rubro = new RubroMateriaPrimaDTO(proveedoresBD[0].id_rubro, proveedoresBD[0].rubroAsociado);
            List<SelectItem<ProveedorDTOBase>> group = new();
            foreach (ProveedorVM proveedor in proveedoresBD)
            {
                if (proveedor.id_rubro != rubro.id)
                {
                    List<SelectItem<ProveedorDTOBase>> newGroup = new(group);
                    groupedDropdown.Add(new SelectItemGroup<RubroMateriaPrimaDTO, ProveedorDTOBase>(rubro.name, rubro, newGroup));
                    rubro = new RubroMateriaPrimaDTO(proveedor.id_rubro, proveedor.rubroAsociado);
                    group.Clear();
                }

                ProveedorDTOBase proveedorDTO = _mapper.Map<ProveedorDTOBase>(proveedor);
                group.Add(new SelectItem<ProveedorDTOBase>(proveedorDTO.name, proveedorDTO));
            }

            groupedDropdown.Add(new SelectItemGroup<RubroMateriaPrimaDTO, ProveedorDTOBase>(rubro.name, rubro, group));

            return groupedDropdown;
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
