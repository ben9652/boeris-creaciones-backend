using BoerisCreaciones.Core.Models;
using BoerisCreaciones.Core.Models.Rubros;
using BoerisCreaciones.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

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
        [Authorize(Roles = "a,sa")]
        public ActionResult<List<RubroMateriaPrimaDTO>> GetRawMaterialCategories()
        {
            List<RubroMateriaPrimaDTO> rubros = new List<RubroMateriaPrimaDTO>();
            try
            {
                rubros = _service.GetRawMaterialsCategories();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }

            return Ok(rubros);
        }

        [HttpPost]
        [Authorize(Roles = "a,sa")]
        public ActionResult<RubroMateriaPrimaDTO> CreateRawMaterialCategory(string name)
        {
            try
            {
                _service.CreateRawMaterialCategory(name);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "a,sa")]
        public ActionResult<RubroMateriaPrimaDTO> ModifyRawMaterialCategory(int id, JsonPatchDocument<RubroMateriaPrimaDTO> patchDoc)
        {
            RubroMateriaPrimaDTO category = _service.GetRawMaterialsCategory(id);
            if (category == null)
                return NotFound("No existe el rubro especificado");

            patchDoc.ApplyTo(category, ModelState);
            if (!TryValidateModel(category))
                return ValidationProblem(ModelState);

            if (patchDoc.Operations.Count == 0)
                return NoContent();

            List<string> attributes = new();
            foreach (Operation<RubroMateriaPrimaDTO> ops in patchDoc.Operations)
                attributes.Add(ops.path);

            try
            {
                _service.ModifyRawMaterialCategory(category, attributes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }

            return Ok(category);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "a,sa")]
        public ActionResult DeleteRawMaterialCategory(int id)
        {
            try
            {
                _service.DeleteRawMaterialCategory(id);
            }
            catch(MySqlException ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(412, "El rubro que se quiere eliminar está siendo utilizado por una materia prima");
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
