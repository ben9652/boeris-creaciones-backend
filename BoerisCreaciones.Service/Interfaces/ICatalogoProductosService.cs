using BoerisCreaciones.Core.Models.Productos;

namespace BoerisCreaciones.Service.Interfaces
{
    public interface ICatalogoProductosService
    {
        public List<ProductosItemDTO> GetProductsItems();
        public ProductosItemDTO GetProductsItem(int id);
        public ProductosItemDTO CreateProductItem(ProductosItemDTO item);
        public ProductosItemDTO UpdateProductItem(ProductosItemDTO item, List<string> attributesToChange);
        public void DeleteProductItem(int id);
    }
}
