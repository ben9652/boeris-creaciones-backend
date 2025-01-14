using AutoMapper;
using BoerisCreaciones.Core.Models.MateriasPrimas;
using BoerisCreaciones.Core.Models.PrimeNG.Dropdown;
using BoerisCreaciones.Core.Models.Rubros;
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

        public List<MateriaPrimaDTO> GetRawMaterialsItems()
        {
            List<MateriaPrimaDTO> materiasPrimas = new List<MateriaPrimaDTO>();
            List<MateriaPrimaVM> materiasPrimasBD = _repository.GetRawMaterialsItems();
            foreach(MateriaPrimaVM materiaPrimaBD in materiasPrimasBD)
                materiasPrimas.Add(_mapper.Map<MateriaPrimaDTO>(materiaPrimaBD));

            return materiasPrimas;
        }

        public MateriaPrimaDTO GetRawMaterialsItem(int id)
        {
            MateriaPrimaVM materiaPrima = _repository.GetRawMaterialsItem(id);
            return _mapper.Map<MateriaPrimaDTO>(materiaPrima);
        }

        public List<SelectItemGroup<RubroMateriaPrimaDTO, MateriaPrimaDTOBase>> GetGroupedDropdown()
        {
            List<SelectItemGroup<RubroMateriaPrimaDTO, MateriaPrimaDTOBase>> groupedDropdown = new();

            List<MateriaPrimaVM> materiasPrimasBD = _repository.GetRawMaterialsItems();

            if (materiasPrimasBD.Count == 0)
                return groupedDropdown;

            materiasPrimasBD = materiasPrimasBD.OrderBy(materiaPrima => materiaPrima.id_rubroMP).ToList();

            RubroMateriaPrimaDTO rubro = new RubroMateriaPrimaDTO(materiasPrimasBD[0].id_rubroMP, materiasPrimasBD[0].rubro);
            List<SelectItem<MateriaPrimaDTOBase>> group = new();
            foreach(MateriaPrimaVM matP in materiasPrimasBD)
            {
                if(matP.id_rubroMP != rubro.id)
                {
                    List<SelectItem<MateriaPrimaDTOBase>> newGroup = new(group);
                    groupedDropdown.Add(new SelectItemGroup<RubroMateriaPrimaDTO, MateriaPrimaDTOBase>(rubro.name, rubro, newGroup));
                    rubro = new RubroMateriaPrimaDTO(matP.id_rubroMP, matP.rubro);
                    group.Clear();
                }

                MateriaPrimaDTOBase materiaPrimaDTO = _mapper.Map<MateriaPrimaDTOBase>(matP);
                group.Add(new SelectItem<MateriaPrimaDTOBase>(materiaPrimaDTO.name, materiaPrimaDTO));
            }

            groupedDropdown.Add(new SelectItemGroup<RubroMateriaPrimaDTO, MateriaPrimaDTOBase>(rubro.name, rubro, group));

            return groupedDropdown;
        }

        public MateriaPrimaDTO CreateRawMaterialItem(MateriaPrimaDTO item)
        {
            MateriaPrimaVM materiaPrima = _mapper.Map<MateriaPrimaVM>(item);
            materiaPrima = _repository.CreateRawMaterialItem(materiaPrima);
            return _mapper.Map<MateriaPrimaDTO>(materiaPrima);
        }

        public MateriaPrimaDTO UpdateRawMaterialItem(MateriaPrimaDTO item, List<string> attributesToChange)
        {
            MateriaPrimaVM materiaPrima = _mapper.Map<MateriaPrimaVM>(item);
            materiaPrima = _repository.UpdateRawMaterialItem(materiaPrima, attributesToChange);
            return _mapper.Map<MateriaPrimaDTO>(materiaPrima);
        }

        public void DeleteRawMaterialItem(int id)
        {
            _repository.DeleteRawMaterialItem(id);
        }
    }
}
