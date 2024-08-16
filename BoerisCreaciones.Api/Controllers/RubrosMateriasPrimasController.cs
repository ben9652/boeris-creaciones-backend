using BoerisCreaciones.Core.Models;
using BoerisCreaciones.Core.Models.Rubros;
using BoerisCreaciones.Service.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;

namespace BoerisCreaciones.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RubrosMateriasPrimasController : Controller
    {
        private readonly IRubrosMateriasPrimasService _service;
        private readonly ILogger<RubrosMateriasPrimasController> _logger;

        public RubrosMateriasPrimasController(IRubrosMateriasPrimasService service, ILogger<RubrosMateriasPrimasController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<List<RubroMateriaPrima>> GetRawMaterialCategories()
        {
            List<RubroMateriaPrima> rubros = new List<RubroMateriaPrima>();
            try
            {
                rubros = _service.GetRawMaterialsCategories();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return Ok(new MensajeSolicitud(ex.Message, true));
            }

            return Ok(rubros);
        }

        [HttpPost]
        public ActionResult CreateRawMaterialCategory(string name)
        {
            try
            {
                _service.CreateRawMaterialCategory(name);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return Ok(new MensajeSolicitud(ex.Message, true));
            }

            return NoContent();
        }

        [HttpPatch("{id}")]
        public ActionResult<MensajeSolicitud> ModifyRawMaterialCategory(int id, JsonPatchDocument<RubroMateriaPrima> patchDoc)
        {
            RubroMateriaPrima category = _service.GetRawMaterialsCategory(id);
            if (category == null)
                return NotFound(new MensajeSolicitud("No existe el rubro especificado", true));

            patchDoc.ApplyTo(category, ModelState);
            if (!TryValidateModel(category))
                return ValidationProblem(ModelState);

            if (patchDoc.Operations.Count == 0)
                return NoContent();

            List<string> attributes = new();
            foreach (Operation<RubroMateriaPrima> ops in patchDoc.Operations)
                attributes.Add(ops.path);

            try
            {
                _service.ModifyRawMaterialCategory(category, attributes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Ok(new MensajeSolicitud(ex.Message, true));
            }

            return Ok(new MensajeSolicitud(category, false));
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteRawMaterialCategory(int id)
        {
            try
            {
                _service.DeleteRawMaterialCategory(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(412, new MensajeSolicitud(ex.Message, true));
            }

            return NoContent();
        }
    }
}
