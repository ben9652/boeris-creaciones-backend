using BoerisCreaciones.Core.Models.MateriasPrimas;
using BoerisCreaciones.Core.Models.PrimeNG.Dropdown;
using BoerisCreaciones.Core.Models.Rubros;

namespace BoerisCreaciones.Service.Interfaces
{
    public interface ICatalogoMateriasPrimasService
    {
        public List<MateriaPrimaDTO> GetRawMaterialsItems();
        public MateriaPrimaDTO GetRawMaterialsItem(int id);
        public List<SelectItemGroup<RubroMateriaPrimaDTO, MateriaPrimaDTOBase>> GetGroupedDropdown();
        public MateriaPrimaDTO CreateRawMaterialItem(MateriaPrimaDTO item);
        public MateriaPrimaDTO UpdateRawMaterialItem(MateriaPrimaDTO item, List<string> attributesToChange);
        public void DeleteRawMaterialItem(int id);
    }
}
