using AutoMapper;
using BoerisCreaciones.Core.Models.Rubros;
using BoerisCreaciones.Repository.Interfaces;
using BoerisCreaciones.Service.Interfaces;

namespace BoerisCreaciones.Service.Services
{
    public class RubrosMateriasPrimasService : IRubrosMateriasPrimasService
    {
        private readonly IRubrosMateriasPrimasRepository _repository;
        private readonly IMapper _mapper;

        public RubrosMateriasPrimasService(IRubrosMateriasPrimasRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public List<RubroMateriaPrimaDTO> GetRawMaterialsCategories()
        {
            List<RubroMateriaPrimaVM> rubrosMateriasPrimasBD = _repository.GetRawMaterialsCategories();
            List<RubroMateriaPrimaDTO> rubrosMateriasPrimas = new List<RubroMateriaPrimaDTO>();
            foreach(RubroMateriaPrimaVM rubroMateriaPrimaBD in rubrosMateriasPrimasBD)
                rubrosMateriasPrimas.Add(_mapper.Map<RubroMateriaPrimaDTO>(rubroMateriaPrimaBD));

            return rubrosMateriasPrimas;
        }

        public RubroMateriaPrimaDTO GetRawMaterialsCategory(int id)
        {
            return _mapper.Map<RubroMateriaPrimaDTO>(_repository.GetRawMaterialsCategory(id));
        }

        public RubroMateriaPrimaDTO CreateRawMaterialCategory(string category)
        {
            category = char.ToUpper(category[0]) + category.Substring(1);
            RubroMateriaPrimaVM rubro = _repository.CreateRawMaterialCategory(category);
            return _mapper.Map<RubroMateriaPrimaDTO>(rubro);
        }

        public void ModifyRawMaterialCategory(RubroMateriaPrimaDTO category, List<string> attributesToChange)
        {
            RubroMateriaPrimaVM categoryDB = _mapper.Map<RubroMateriaPrimaVM>(category);

            categoryDB.nombre = char.ToUpper(categoryDB.nombre[0]) + categoryDB.nombre.Substring(1);
            _repository.ModifyRawMaterialCategory(categoryDB, attributesToChange);
        }

        public void DeleteRawMaterialCategory(int id)
        {
            _repository.DeleteRawMaterialCategory(id);
        }
    }
}
