using BoerisCreaciones.Core.Models.MateriasPrimas;

namespace BoerisCreaciones.Repository.Interfaces
{
    public interface ICatalogoMateriasPrimasRepository
    {
        public List<MateriasPrimasItemVM> GetRawMaterialsItems();
        public MateriasPrimasItemVM GetRawMaterialsItem(int id);
        public MateriasPrimasItemVM CreateRawMaterialItem(MateriasPrimasItemVM item);
        public MateriasPrimasItemVM UpdateRawMaterialItem(MateriasPrimasItemVM item, List<string> attributesToChange);
        public void DeleteRawMaterialItem(int id);
    }
}
