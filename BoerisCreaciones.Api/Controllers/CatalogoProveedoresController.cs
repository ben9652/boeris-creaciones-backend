using BoerisCreaciones.Core.Models.Localidades;
using BoerisCreaciones.Core.Models.PrimeNG.Dropdown;
using BoerisCreaciones.Core.Models.Proveedores;
using BoerisCreaciones.Core.Models.Rubros;
using BoerisCreaciones.Core.Models.Sucursales;
using BoerisCreaciones.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

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
#if RELEASE
        [Authorize(Roles = "a,sa")]
#endif
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
                return NotFound(new { ex.Message });
            }

            return Ok(catalogoProveedores);
        }

        [HttpGet("{id}")]
#if RELEASE
        [Authorize(Roles = "a,sa")]
#endif
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
                return NotFound(new { ex.Message });
            }

            return Ok(proveedor);
        }

        [HttpGet("Dropdown")]
#if RELEASE
        [Authorize]
#endif
        public ActionResult GetGroupedDropdown()
        {
            List<SelectItemGroup<RubroMateriaPrimaDTO, ProveedorDTO>> dropdownAgrupado = new();

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

        [HttpGet("Dropdown/{categories}")]
#if RELEASE
        [Authorize]
#endif
        public ActionResult GetGroupedDropdownWithFilters(string categories)
        {
            // Si `categories` no es una serie de números enteros separados por guiones, devolver un BadRequest
            if (!Regex.IsMatch(categories, @"^(\d+-)*\d+$"))
                return BadRequest(new { Message = "El formato de las categorías es inválido" });

            List<int> categoriesList = new();
            if (!string.IsNullOrEmpty(categories))
            {
                string[] categoriesArray = categories.Split('-');
                foreach (string category in categoriesArray)
                    categoriesList.Add(int.Parse(category));
            }

            List<SelectItemGroup<RubroMateriaPrimaDTO, ProveedorDTO>> dropdownAgrupado = new();

            try
            {
                dropdownAgrupado = _service.GetGroupedDropdown(categoriesList);
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
        public ActionResult CreateProvider(ProveedorDTO proveedor)
        {
            try
            {
                proveedor = _service.CreateProvider(proveedor);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { ex.Message });
            }

            return Ok(proveedor);
        }

        [HttpPatch("{id}")]
#if RELEASE
        [Authorize(Roles = "a,sa")]
#endif
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
                return BadRequest(new { ex.Message });
            }

            return Ok(provider);
        }

        [HttpDelete("{id}")]
#if RELEASE
        [Authorize(Roles = "a,sa")]
#endif
        public ActionResult DeleteProvider(int id)
        {
            try
            {
                _service.DeleteProvider(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(412, new { ex.Message });
            }

            return NoContent();
        }
    }
}
