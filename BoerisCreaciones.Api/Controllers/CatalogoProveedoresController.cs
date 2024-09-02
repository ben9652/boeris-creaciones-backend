using BoerisCreaciones.Core.Models.Proveedores;
using BoerisCreaciones.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;

namespace BoerisCreaciones.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogoProveedoresController : Controller
    {
        private readonly ICatalogoProveedoresService _service;
        private readonly ILogger<CatalogoProveedoresController> _logger;

        public CatalogoProveedoresController(ICatalogoProveedoresService service, ILogger<CatalogoProveedoresController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "a,sa")]
        public ActionResult GetProviders()
        {
            List<ProveedorDTO> catalogoProveedores = new List<ProveedorDTO>();

            try
            {
                catalogoProveedores = _service.GetProviders();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }

            return Ok(catalogoProveedores);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "a,sa")]
        public ActionResult GetProvider(int id)
        {
            ProveedorDTO proveedor = null;

            try
            {
                proveedor = _service.GetProvider(id);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }

            return Ok(proveedor);
        }

        [HttpPost]
        [Authorize(Roles = "a,sa")]
        public ActionResult CreateProvider(ProveedorDTO proveedor)
        {
            try
            {
                proveedor = _service.CreateProvider(proveedor);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }

            return Ok(proveedor);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "a,sa")]
        public ActionResult UpdateProvider(int id, JsonPatchDocument<ProveedorDTO> patchDoc)
        {
            ProveedorDTO provider = _service.GetProvider(id);
            if (provider == null)
                return NotFound("No existe el proveedor especificado");

            patchDoc.ApplyTo(provider, ModelState);
            if (!TryValidateModel(provider))
                return ValidationProblem(ModelState);

            if (patchDoc.Operations.Count == 0)
                return NoContent();

            List<string> attributes = new();
            foreach (Operation<ProveedorDTO> ops in patchDoc.Operations)
                attributes.Add(ops.path);

            try
            {
                provider = _service.UpdateProvider(provider, attributes);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }

            return Ok(provider);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "a,sa")]
        public ActionResult DeleteProvider(int id)
        {
            try
            {
                _service.DeleteProvider(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(412, ex.Message);
            }

            return NoContent();
        }
    }
}
