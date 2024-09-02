using BoerisCreaciones.Core.Models.Rubros;

namespace BoerisCreaciones.Repository.Interfaces
{
    public interface IRubrosProductosRepository
    {
        public List<RubroProductoVM> GetProductsCategories();
        public RubroProductoVM GetProductsCategory(int id);
        public RubroProductoVM CreateProductsCategory(string category);
        public void ModifyProductsCategory(RubroProductoVM category, List<string> attributesToChange);
        public void DeleteProductsCategory(int id);
    }
}
