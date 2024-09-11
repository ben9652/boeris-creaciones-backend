using BoerisCreaciones.Core.Models.Rubros;

namespace BoerisCreaciones.Service.Interfaces
{
    public interface IRubrosMateriasPrimasService
    {
        public List<RubroMateriaPrimaDTO> GetRawMaterialsCategories();
        public RubroMateriaPrimaDTO GetRawMaterialsCategory(int id);
        public RubroMateriaPrimaDTO CreateRawMaterialCategory(string category);
        public void ModifyRawMaterialCategory(RubroMateriaPrimaDTO category, List<string> attributesToChange);
        public void DeleteRawMaterialCategory(int id);
    }
}
