using BoerisCreaciones.Core.Models.Localidades;
using BoerisCreaciones.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BoerisCreaciones.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocalidadesController : Controller
    {
        private readonly ILocalidadesService _service;
        private readonly ILogger<LocalidadesController> _logger;

        public LocalidadesController(ILocalidadesService service, ILogger<LocalidadesController> logger)
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
            List<LocalidadDTO> provincias = new List<LocalidadDTO>();

            try
            {
                provincias = _service.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }

            return Ok(provincias);
        }

        [HttpGet("Expanded")]
#if RELEASE
        [Authorize(Roles = "a")]
#endif
        public ActionResult GetAllFull()
        {
            List<LocalidadExpandedDTO> localidades = new List<LocalidadExpandedDTO>();

            try
            {
                localidades = _service.GetAllLocalitiesWithBranches();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }

            return Ok(localidades);
        }

        [HttpPost]
#if RELEASE
        [Authorize(Roles = "a,sa")]
#endif
        public ActionResult Create(LocalidadDTO localidad)
        {
            try
            {
                localidad = _service.Create(localidad);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }

            return Ok(localidad);
        }

        [HttpPatch("{id}")]
#if RELEASE
        [Authorize(Roles = "a,sa")]
#endif
        public ActionResult Update(int id, JsonPatchDocument<LocalidadDTO> patchDoc)
        {
            LocalidadDTO localidad = _service.GetById(id);
            if (localidad == null)
                return NotFound("No existe la provincia especificada");

            patchDoc.ApplyTo(localidad, ModelState);
            if (!TryValidateModel(localidad))
                return ValidationProblem(ModelState);

            if (patchDoc.Operations.Count == 0)
                return NoContent();

            List<string> attributes = new();
            foreach (Operation<LocalidadDTO> ops in patchDoc.Operations)
                attributes.Add(ops.path);

            try
            {
                localidad = _service.Update(localidad, attributes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }

            return Ok(localidad);
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
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(412, ex.Message);
            }

            return NoContent();
        }
    }
}
