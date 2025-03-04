using BoerisCreaciones.Core.Models.Productos;
using BoerisCreaciones.Service.Helpers;
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
        private readonly IWebHostEnvironment _env;

        public CatalogoProductosController(ILogger<CatalogoProductosController> logger, ICatalogoProductosService service, IWebHostEnvironment env)
        {
            _logger = logger;
            _service = service;
            _env = env;

            var webRootPath = _env.WebRootPath;
            if (string.IsNullOrEmpty(webRootPath))
            {
                webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                _env.WebRootPath = webRootPath;
            }
        }

        [HttpGet]
#if RELEASE
        [Authorize(Roles = "a,sa")]
#endif
        public ActionResult<List<ProductoDTO>> GetAllProducts()
        {
            List<ProductoDTO> productos = new List<ProductoDTO>();

            try
            {
                productos = _service.GetProductsItems();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(new { ex.Message });
            }

            return Ok(productos);
        }

        [HttpGet("{id}")]
#if RELEASE
        [Authorize(Roles = "a,sa")]
#endif
        public ActionResult<ProductoDTO> GetProductById(int id)
        {
            ProductoDTO producto = null;

            try
            {
                producto = _service.GetProductsItem(id);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(new { ex.Message });
            }

            return Ok(producto);
        }

        [HttpPost]
#if RELEASE
        [Authorize(Roles = "a,sa")]
#endif
        public ActionResult<ProductoDTO> CreateProduct(ProductoDTO item)
        {
            try
            {
                item = _service.CreateProductItem(item);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(new { ex.Message });
            }

            return Ok(item);
        }

        [HttpPost("upload-image")]
#if RELEASE
        [Authorize(Roles = "a,sa")]
#endif
        public async Task<IActionResult> UploadImage()
        {
            IFormFileCollection files = Request.Form.Files;

            string url;
            string controllerName = "CatalogoProductos";

            try
            {
                string fileName = await MultimediaManaging.UploadImage(files[0], _env.WebRootPath, controllerName);

                // Devolver la URL o la ruta del archivo guardado
                url = $"https://{Request.Host}:9354/{controllerName}/{fileName}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { ex.Message });
            }

            return Ok(url);
        }

        [HttpDelete("delete-image/{imagePath}")]
#if RELEASE
        [Authorize(Roles = "a,sa")]
#endif
        public IActionResult DeleteImage(string imagePath)
        {
            string controllerName = "CatalogoProductos";

            bool result = MultimediaManaging.DeleteImage(imagePath, _env.WebRootPath, controllerName);

            return Ok(result);
        }

        [HttpPatch("{id}")]
#if RELEASE
        [Authorize(Roles = "a,sa")]
#endif
        public ActionResult UpdateProduct(int id, JsonPatchDocument<ProductoDTO> patchDoc)
        {
            ProductoDTO item = _service.GetProductsItem(id);
            if (item == null)
                return NotFound("No existe el producto especificado");

            patchDoc.ApplyTo(item, ModelState);
            if (!TryValidateModel(item))
                return ValidationProblem(ModelState);

            if (patchDoc.Operations.Count == 0)
                return NoContent();

            List<string> attributes = new();
            foreach (Operation<ProductoDTO> ops in patchDoc.Operations)
                attributes.Add(ops.path);

            try
            {
                item = _service.UpdateProductItem(item, attributes);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { ex.Message });
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
                return StatusCode(412, new { ex.Message });
            }

            return NoContent();
        }
    }
}
