using AutoMapper;
using BoerisCreaciones.Core.Models.MateriasPrimas;
using BoerisCreaciones.Repository.Interfaces;
using BoerisCreaciones.Service.Interfaces;

namespace BoerisCreaciones.Service.Services
{
    public class CatalogoMateriasPrimasService : ICatalogoMateriasPrimasService
    {
        private readonly ICatalogoMateriasPrimasRepository _repository;
        private readonly IMapper _mapper;

        public CatalogoMateriasPrimasService(ICatalogoMateriasPrimasRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public List<MateriasPrimasItemDTO> GetRawMaterialsItems()
        {
            List<MateriasPrimasItemDTO> materiasPrimas = new List<MateriasPrimasItemDTO>();
            List<MateriasPrimasItemVM> materiasPrimasBD = _repository.GetRawMaterialsItems();
            foreach(MateriasPrimasItemVM materiaPrimaBD in materiasPrimasBD)
                materiasPrimas.Add(_mapper.Map<MateriasPrimasItemDTO>(materiaPrimaBD));

            return materiasPrimas;
        }

        public MateriasPrimasItemDTO GetRawMaterialsItem(int id)
        {
            MateriasPrimasItemVM materiaPrima = _repository.GetRawMaterialsItem(id);
            return _mapper.Map<MateriasPrimasItemDTO>(materiaPrima);
        }

        public MateriasPrimasItemDTO CreateRawMaterialItem(MateriasPrimasItemDTO item)
        {
            MateriasPrimasItemVM materiaPrima = _mapper.Map<MateriasPrimasItemVM>(item);
            materiaPrima = _repository.CreateRawMaterialItem(materiaPrima);
            return _mapper.Map<MateriasPrimasItemDTO>(materiaPrima);
        }

        public MateriasPrimasItemDTO UpdateRawMaterialItem(MateriasPrimasItemDTO item, List<string> attributesToChange)
        {
            MateriasPrimasItemVM materiaPrima = _mapper.Map<MateriasPrimasItemVM>(item);
            materiaPrima = _repository.UpdateRawMaterialItem(materiaPrima, attributesToChange);
            return _mapper.Map<MateriasPrimasItemDTO>(materiaPrima);
        }

        public void DeleteRawMaterialItem(int id)
        {
            _repository.DeleteRawMaterialItem(id);
        }
    }
}
