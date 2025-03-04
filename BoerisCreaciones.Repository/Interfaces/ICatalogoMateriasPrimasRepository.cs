using BoerisCreaciones.Core.Models.MateriasPrimas;

namespace BoerisCreaciones.Repository.Interfaces
{
    public interface ICatalogoMateriasPrimasRepository
    {
        public List<MateriaPrimaVM> GetRawMaterialsItems(List<int>? categories = null);
        public MateriaPrimaVM GetRawMaterialsItem(int id);
        public MateriaPrimaVM CreateRawMaterialItem(MateriaPrimaVM item);
        public MateriaPrimaVM UpdateRawMaterialItem(MateriaPrimaVM item, List<string> attributesToChange);
        public void DeleteRawMaterialItem(int id);
    }
}
