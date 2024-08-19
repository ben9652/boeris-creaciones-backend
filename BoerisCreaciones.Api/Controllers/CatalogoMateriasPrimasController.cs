using BoerisCreaciones.Core.Models.MateriasPrimas;
using BoerisCreaciones.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;

namespace BoerisCreaciones.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogoMateriasPrimasController : Controller
    {
        private readonly ICatalogoMateriasPrimasService _service;
        private readonly ILogger<CatalogoMateriasPrimasController> _logger;

        public CatalogoMateriasPrimasController(ICatalogoMateriasPrimasService service, ILogger<CatalogoMateriasPrimasController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "a,sa")]
        public ActionResult<List<MateriasPrimasItemDTO>> Get()
        {
            List<MateriasPrimasItemDTO> catalogoMateriasPrimas = new List<MateriasPrimasItemDTO>();

            try
            {
                catalogoMateriasPrimas = _service.GetRawMaterialsItems();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }

            return Ok(catalogoMateriasPrimas);
        }

        [HttpPost]
        [Authorize(Roles = "a,sa")]
        public ActionResult<MateriasPrimasItemDTO> Create(MateriasPrimasItemDTO itemMateriaPrima)
        {
            char origen = itemMateriaPrima.source;
            if (origen != 'C' && origen != 'E' && origen != 'P')
                return BadRequest("El valor del origen es inválido");

            try
            {
                itemMateriaPrima = _service.CreateRawMaterialItem(itemMateriaPrima);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }

            return Ok(itemMateriaPrima);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "a,sa")]
        public ActionResult Update(int id, JsonPatchDocument<MateriasPrimasItemDTO> patchDoc)
        {
            MateriasPrimasItemDTO item = _service.GetRawMaterialsItem(id);
            if (item == null)
                return NotFound("No existe el rubro especificado");

            patchDoc.ApplyTo(item, ModelState);
            if (!TryValidateModel(item))
                return ValidationProblem(ModelState);

            if (patchDoc.Operations.Count == 0)
                return NoContent();

            List<string> attributes = new();
            foreach (Operation<MateriasPrimasItemDTO> ops in patchDoc.Operations)
                attributes.Add(ops.path);

            try
            {
                item = _service.UpdateRawMaterialItem(item, attributes);
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
        public ActionResult Delete(int id)
        {
            try
            {
                _service.DeleteRawMaterialItem(id);
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
