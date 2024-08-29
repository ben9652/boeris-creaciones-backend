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
        [Authorize(Roles = "a,sa")]
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
        [Authorize(Roles = "a,sa")]
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
        [Authorize(Roles = "a,sa")]
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
        [Authorize(Roles = "a,sa")]
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
        [Authorize(Roles = "a,sa")]
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
