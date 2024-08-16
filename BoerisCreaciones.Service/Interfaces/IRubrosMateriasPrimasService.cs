using BoerisCreaciones.Core.Models.Rubros;

namespace BoerisCreaciones.Service.Interfaces
{
    public interface IRubrosMateriasPrimasService
    {
        public List<RubroMateriaPrima> GetRawMaterialsCategories();
        public RubroMateriaPrima GetRawMaterialsCategory(int id);
        public void CreateRawMaterialCategory(string category);
        public void ModifyRawMaterialCategory(RubroMateriaPrima category, List<string> attributesToChange);
        public void DeleteRawMaterialCategory(int id);
    }
}
