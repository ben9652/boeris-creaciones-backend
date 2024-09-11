using BoerisCreaciones.Core.Models.Rubros;

namespace BoerisCreaciones.Repository.Interfaces
{
    public interface IRubrosMateriasPrimasRepository
    {
        public List<RubroMateriaPrimaVM> GetRawMaterialsCategories();
        public RubroMateriaPrimaVM GetRawMaterialsCategory(int id);
        public RubroMateriaPrimaVM CreateRawMaterialCategory(string category);
        public void ModifyRawMaterialCategory(RubroMateriaPrimaVM category, List<string> attributesToChange);
        public void DeleteRawMaterialCategory(int id);
    }
}
