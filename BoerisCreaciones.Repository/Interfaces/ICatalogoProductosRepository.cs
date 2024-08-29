using BoerisCreaciones.Core.Models.Productos;

namespace BoerisCreaciones.Repository.Interfaces
{
    public interface ICatalogoProductosRepository
    {
        public List<ProductosItemVM> GetProductsItems();
        public ProductosItemVM GetProductsItem(int id);
        public ProductosItemVM CreateProductItem(ProductosItemVM item);
        public ProductosItemVM UpdateProductItem(ProductosItemVM item, List<string> attributesToChange);
        public void DeleteProductItem(int id);
    }
}
