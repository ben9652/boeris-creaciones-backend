using BoerisCreaciones.Core.Models.Localidades;
using BoerisCreaciones.Core.Models.PrimeNG.Dropdown;
using BoerisCreaciones.Core.Models.Sucursales;
using BoerisCreaciones.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;

namespace BoerisCreaciones.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogoSucursalesController : Controller
    {
        private readonly ICatalogoSucursalesService _service;
        private readonly ILogger<CatalogoSucursalesController> _logger;

        public CatalogoSucursalesController(ICatalogoSucursalesService service, ILogger<CatalogoSucursalesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
#if RELEASE
        [Authorize(Roles = "a,sa")]
#endif
        public ActionResult GetAll()
        {
            List<SucursalDTO> sucursales = new List<SucursalDTO>();

            try
            {
                sucursales = _service.GetAll();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(new { ex.Message });
            }

            return Ok(sucursales);
        }

        [HttpGet("Dropdown")]
#if RELEASE
        [Authorize]
#endif
        public ActionResult GetGroupedDropdown()
        {
            List<SelectItemGroup<LocalidadDTOBase, SucursalDTO>> dropdownAgrupado = new();

            try
            {
                dropdownAgrupado = _service.GetGroupedDropdown();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { ex.Message });
            }

            return Ok(dropdownAgrupado);
        }

        [HttpPost]
#if RELEASE
        [Authorize(Roles = "a,sa")]
#endif
        public ActionResult Create(SucursalDTO sucursal)
        {
            try
            {
                sucursal = _service.Create(sucursal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { ex.Message });
            }

            return Ok(sucursal);
        }

        [HttpPatch("{id}")]
#if RELEASE
        [Authorize(Roles = "a,sa")]
#endif
        public ActionResult Update(int id, JsonPatchDocument<SucursalDTO> patchDoc)
        {
            SucursalDTO sucursal = _service.GetById(id);
            if (sucursal == null)
                return NotFound("No existe la sucursal especificada");

            patchDoc.ApplyTo(sucursal, ModelState);
            if (!TryValidateModel(sucursal))
                return ValidationProblem(ModelState);

            if (patchDoc.Operations.Count == 0)
                return NoContent();

            List<string> attributes = new();
            foreach (Operation<SucursalDTO> ops in patchDoc.Operations)
                attributes.Add(ops.path);

            try
            {
                sucursal = _service.Update(sucursal, attributes);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { ex.Message });
            }

            return Ok(sucursal);
        }

        [HttpDelete("{id}")]
#if RELEASE
        [Authorize(Roles = "a,sa")]
#endif
        public ActionResult Delete(int id)
        {
            try
            {
                _service.Delete(id);
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
