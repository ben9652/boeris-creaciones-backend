using AutoMapper;
using BoerisCreaciones.Core.Models.Unidades;
using BoerisCreaciones.Repository.Interfaces;
using BoerisCreaciones.Service.Interfaces;

namespace BoerisCreaciones.Service.Services
{
    public class UnidadesService : IUnidadesService
    {
        private readonly IUnidadesRepository _unidadesRepository;
        private readonly IMapper _mapper;

        public UnidadesService(IUnidadesRepository unidadesRepository, IMapper mapper)
        {
            _unidadesRepository = unidadesRepository;
            _mapper = mapper;
        }

        public List<UnidadDTO> GetUnits()
        {
            List<UnidadVM> unidadesBD = _unidadesRepository.GetUnits();
            List<UnidadDTO> unidades = new List<UnidadDTO>();
            foreach (UnidadVM unidadBD in unidadesBD)
                unidades.Add(_mapper.Map<UnidadDTO>(unidadBD));

            return unidades;
        }

        public UnidadDTO GetUnit(int id)
        {
            UnidadVM unidadBD = _unidadesRepository.GetUnit(id);
            return _mapper.Map<UnidadDTO>(unidadBD);
        }
    }
}
