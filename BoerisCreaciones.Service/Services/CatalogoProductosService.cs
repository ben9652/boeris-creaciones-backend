using AutoMapper;
using BoerisCreaciones.Core.Models.Productos;
using BoerisCreaciones.Core.Models.Rubros;
using BoerisCreaciones.Repository.Interfaces;
using BoerisCreaciones.Service.Interfaces;

namespace BoerisCreaciones.Service.Services
{
    public class CatalogoProductosService : ICatalogoProductosService
    {
        private readonly ICatalogoProductosRepository _repository;
        private readonly IRubrosProductosRepository _repositoryRubrosP;
        private readonly IMapper _mapper;

        public CatalogoProductosService(ICatalogoProductosRepository repository, IRubrosProductosRepository repositoryRubrosP, IMapper mapper)
        {
            _repository = repository;
            _repositoryRubrosP = repositoryRubrosP;
            _mapper = mapper;
        }

        public List<ProductosItemDTO> GetProductsItems()
        {
            List<ProductosItemDTO> productos = new List<ProductosItemDTO>();
            List<ProductosItemVM> productosDB = _repository.GetProductsItems();
            foreach(ProductosItemVM productoDB in productosDB)
                productos.Add(_mapper.Map<ProductosItemDTO>(productoDB));

            return productos;
        }

        public ProductosItemDTO GetProductsItem(int id)
        {
            ProductosItemVM productoDB = _repository.GetProductsItem(id);
            return _mapper.Map<ProductosItemDTO>(productoDB);
        }

        public ProductosItemDTO CreateProductItem(ProductosItemDTO item)
        {
            RubroProductoVM rubro = _repositoryRubrosP.CreateProductsCategory(item.name);
            item.category = new RubroProductoDTO(rubro.id_rubroP, rubro.nombre);
            ProductosItemVM productoDB = _mapper.Map<ProductosItemVM>(item);
            productoDB = _repository.CreateProductItem(productoDB);
            return _mapper.Map<ProductosItemDTO>(productoDB);
        }

        public ProductosItemDTO UpdateProductItem(ProductosItemDTO item, List<string> attributesToChange)
        {
            ProductosItemVM productoDB = _mapper.Map<ProductosItemVM>(item);
            productoDB = _repository.UpdateProductItem(productoDB, attributesToChange);
            productoDB.rubro = productoDB.nombre;
            _repositoryRubrosP.ModifyProductsCategory(new RubroProductoVM(productoDB.id_rubroP, productoDB.rubro), new List<string> { "nombre" });
            return _mapper.Map<ProductosItemDTO>(productoDB);
        }

        public void DeleteProductItem(int id)
        {
            ProductosItemVM producto = _repository.GetProductsItem(id);
            _repository.DeleteProductItem(id);
            _repositoryRubrosP.DeleteProductsCategory(producto.id_rubroP);
        }
    }
}
