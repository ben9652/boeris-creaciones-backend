using BoerisCreaciones.Core.Models.Productos;

namespace BoerisCreaciones.Repository.Interfaces
{
    public interface ICatalogoProductosRepository
    {
        public List<ProductoVM> GetProductsItems();
        public ProductoVM GetProductsItem(int id);
        public ProductoVM CreateProductItem(ProductoVM item);
        public ProductoVM UpdateProductItem(ProductoVM item, List<string> attributesToChange);
        public void DeleteProductItem(int id);
    }
}
