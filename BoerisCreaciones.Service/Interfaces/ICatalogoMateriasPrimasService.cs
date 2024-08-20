using BoerisCreaciones.Core.Models.MateriasPrimas;

namespace BoerisCreaciones.Service.Interfaces
{
    public interface ICatalogoMateriasPrimasService
    {
        public List<MateriasPrimasItemDTO> GetRawMaterialsItems();
        public MateriasPrimasItemDTO GetRawMaterialsItem(int id);
        public MateriasPrimasItemDTO CreateRawMaterialItem(MateriasPrimasItemDTO item);
        public MateriasPrimasItemDTO UpdateRawMaterialItem(MateriasPrimasItemDTO item, List<string> attributesToChange);
        public void DeleteRawMaterialItem(int id);
    }
}
