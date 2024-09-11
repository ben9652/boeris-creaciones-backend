using AutoMapper;
using BoerisCreaciones.Core.Models.Localidades;
using BoerisCreaciones.Core.Models.Sucursales;
using BoerisCreaciones.Repository.Interfaces;
using BoerisCreaciones.Service.Interfaces;

namespace BoerisCreaciones.Service.Services
{
    public class LocalidadesService : ILocalidadesService
    {
        private readonly ILocalidadesRepository _repository;
        private readonly ICatalogoSucursalesRepository _sucursalesRepository;
        private readonly IMapper _mapper;

        public LocalidadesService(ILocalidadesRepository repository, ICatalogoSucursalesRepository sucursalesRepository, IMapper mapper)
        {
            _repository = repository;
            _sucursalesRepository = sucursalesRepository;
            _mapper = mapper;
        }

        public List<LocalidadDTO> GetAll()
        {
            List<LocalidadVM> localidadesBD = _repository.GetAll();
            List<LocalidadDTO> localidades = new List<LocalidadDTO>();
            foreach (LocalidadVM localidadBD in localidadesBD)
                localidades.Add(_mapper.Map<LocalidadDTO>(localidadBD));

            return localidades;
        }

        public List<LocalidadExpandedDTO> GetAllLocalitiesWithBranches()
        {
            List<LocalidadVM> localidadesBD = _repository.GetAll();
            List<LocalidadExpandedDTO> localidades = new List<LocalidadExpandedDTO>();
            foreach (LocalidadVM localidadBD in localidadesBD)
                localidades.Add(_mapper.Map<LocalidadExpandedDTO>(localidadBD));

            foreach(LocalidadExpandedDTO localidad in localidades)
            {
                List<SucursalVM> sucursales = _sucursalesRepository.GetByIdLocality(localidad.id);
                foreach (SucursalVM sucursal in sucursales)
                    localidad.branches.Add(_mapper.Map<SucursalExpandedDTO>(sucursal));
            }

            return localidades;
        }

        public LocalidadDTO GetById(int id)
        {
            LocalidadVM localidadBD = _repository.GetById(id);
            LocalidadDTO localidad = _mapper.Map<LocalidadDTO>(localidadBD);
            return localidad;
        }

        public LocalidadExpandedDTO GetLocalityWithBranches(int id)
        {
            throw new NotImplementedException();
        }

        public LocalidadDTO Create(LocalidadDTO localidad)
        {
            LocalidadVM localidadBD = _mapper.Map<LocalidadVM>(localidad);
            localidadBD = _repository.Create(localidadBD);
            return _mapper.Map<LocalidadDTO>(localidadBD);
        }

        public LocalidadDTO Update(LocalidadDTO localidad, List<string> attributes)
        {
            LocalidadVM localidadBD = _mapper.Map<LocalidadVM>(localidad);
            localidadBD = _repository.Update(localidadBD, attributes);
            return _mapper.Map<LocalidadDTO>(localidadBD);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }
    }
}
