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

        public List<ProductoDTO> GetProductsItems()
        {
            List<ProductoDTO> productos = new List<ProductoDTO>();
            List<ProductoVM> productosDB = _repository.GetProductsItems();
            foreach(ProductoVM productoDB in productosDB)
                productos.Add(_mapper.Map<ProductoDTO>(productoDB));

            return productos;
        }

        public ProductoDTO GetProductsItem(int id)
        {
            ProductoVM productoDB = _repository.GetProductsItem(id);
            return _mapper.Map<ProductoDTO>(productoDB);
        }

        public ProductoDTO CreateProductItem(ProductoDTO item)
        {
            RubroProductoVM rubro = _repositoryRubrosP.CreateProductsCategory(item.name);
            item.category = new RubroProductoDTO(rubro.id_rubroP, rubro.nombre);
            ProductoVM productoDB = _mapper.Map<ProductoVM>(item);
            productoDB = _repository.CreateProductItem(productoDB);
            return _mapper.Map<ProductoDTO>(productoDB);
        }

        public ProductoDTO UpdateProductItem(ProductoDTO item, List<string> attributesToChange)
        {
            ProductoVM productoDB = _mapper.Map<ProductoVM>(item);
            productoDB = _repository.UpdateProductItem(productoDB, attributesToChange);
            if (attributesToChange.Find(str => str == "name") != null)
            {
                productoDB.rubro = productoDB.nombre;
                _repositoryRubrosP.ModifyProductsCategory(new RubroProductoVM(productoDB.id_rubroP, productoDB.rubro), new List<string> { "nombre" });
            }
            return _mapper.Map<ProductoDTO>(productoDB);
        }

        public void DeleteProductItem(int id)
        {
            ProductoVM producto = _repository.GetProductsItem(id);
            _repository.DeleteProductItem(id);
            _repositoryRubrosP.DeleteProductsCategory(producto.id_rubroP);
        }
    }
}
