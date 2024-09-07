using AutoMapper;
using BoerisCreaciones.Core.Models.Localidades;
using BoerisCreaciones.Repository.Interfaces;
using BoerisCreaciones.Service.Interfaces;

namespace BoerisCreaciones.Service.Services
{
    public class LocalidadesService : ILocalidadesService
    {
        private readonly ILocalidadesRepository _repository;
        private readonly IMapper _mapper;

        public LocalidadesService(ILocalidadesRepository repository, IMapper mapper)
        {
            _repository = repository;
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

        public LocalidadDTO GetById(int id)
        {
            LocalidadVM localidadBD = _repository.GetById(id);
            LocalidadDTO localidad = _mapper.Map<LocalidadDTO>(localidadBD);
            return localidad;
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
