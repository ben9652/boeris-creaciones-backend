using BoerisCreaciones.Core.Models.Productos;
using BoerisCreaciones.Service.Interfaces;
using DotNetEnv;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp.Formats.Jpeg;

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

        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile picture)
        {
            if (picture == null || picture.Length == 0)
            {
                return BadRequest("No se proporcionó una imagen válida.");
            }

            var webRootPath = _env.WebRootPath;
            if (string.IsNullOrEmpty(webRootPath))
            {
                webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            string controllerName = "CatalogoProductos";

            // Combinar ruta
            var uploadsPath = Path.Combine(webRootPath, controllerName);
            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }

            string url;

            try
            {
                // Generar un nombre único para la imagen
                string? extension = Path.GetExtension(picture.FileName);
                var fileName = Guid.NewGuid().ToString() + extension;

                // Ruta para guardar la imagen en el servidor
                var filePath = Path.Combine(uploadsPath, fileName);

                // Cargar la imagen y procesarla (redimensionar y comprimir)
                using (var image = Image.Load(picture.OpenReadStream()))
                {
                    // Redimensionar la imagen si es necesario
                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Mode = ResizeMode.Max,
                        Size = new Size(500, 500)  // Tamaño máximo de 800x800
                    }));

                    // Guardar la imagen comprimida con calidad del 70%
                    await image.SaveAsync(filePath, new JpegEncoder { Quality = 70 });
                }

                // Devolver la URL o la ruta del archivo guardado
                url = $"{Request.Scheme}://{Request.Host}/{controllerName}/{fileName}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }

            return Ok(new { url });
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
