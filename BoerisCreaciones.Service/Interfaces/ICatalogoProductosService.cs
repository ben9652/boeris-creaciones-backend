using BoerisCreaciones.Core.Models.Productos;

namespace BoerisCreaciones.Service.Interfaces
{
    public interface ICatalogoProductosService
    {
        public List<ProductoDTO> GetProductsItems();
        public ProductoDTO GetProductsItem(int id);
        public ProductoDTO CreateProductItem(ProductoDTO item);
        public ProductoDTO UpdateProductItem(ProductoDTO item, List<string> attributesToChange);
        public void DeleteProductItem(int id);
    }
}
