using BoerisCreaciones.Core.Models.Productos;
using BoerisCreaciones.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;

namespace BoerisCreaciones.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogoProductosController : Controller
    {
        private readonly ILogger<CatalogoProductosController> _logger;
        private readonly ICatalogoProductosService _service;

        public CatalogoProductosController(ILogger<CatalogoProductosController> logger, ICatalogoProductosService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
#if RELEASE
        [Authorize(Roles = "a,sa")]
#endif
        public ActionResult<List<ProductosItemDTO>> GetAllProducts()
        {
            List<ProductosItemDTO> productos = new List<ProductosItemDTO>();

            try
            {
                productos = _service.GetProductsItems();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }

            return Ok(productos);
        }

        [HttpGet("{id}")]
#if RELEASE
        [Authorize(Roles = "a,sa")]
#endif
        public ActionResult<ProductosItemDTO> GetProductById(int id)
        {
            ProductosItemDTO producto = null;

            try
            {
                producto = _service.GetProductsItem(id);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }

            return Ok(producto);
        }

        [HttpPost]
#if RELEASE
        [Authorize(Roles = "a,sa")]
#endif
        public ActionResult<ProductosItemDTO> CreateProduct(ProductosItemDTO item)
        {
            try
            {
                item = _service.CreateProductItem(item);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }

            return Ok(item);
        }

        [HttpPatch("{id}")]
#if RELEASE
        [Authorize(Roles = "a,sa")]
#endif
        public ActionResult UpdateProduct(int id, JsonPatchDocument<ProductosItemDTO> patchDoc)
        {
            ProductosItemDTO item = _service.GetProductsItem(id);
            if (item == null)
                return NotFound("No existe el producto especificado");

            patchDoc.ApplyTo(item, ModelState);
            if (!TryValidateModel(item))
                return ValidationProblem(ModelState);

            if (patchDoc.Operations.Count == 0)
                return NoContent();

            List<string> attributes = new();
            foreach (Operation<ProductosItemDTO> ops in patchDoc.Operations)
                attributes.Add(ops.path);

            try
            {
                item = _service.UpdateProductItem(item, attributes);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }

            return Ok(item);
        }

        [HttpDelete("{id}")]
#if RELEASE
        [Authorize(Roles = "a,sa")]
#endif
        public ActionResult DeleteProduct(int id)
        {
            try
            {
                _service.DeleteProductItem(id);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(412, ex.Message);
            }

            return NoContent();
        }
    }
}
