using BoerisCreaciones.Core.Models.Socio;
using BoerisCreaciones.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoerisCreaciones.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesSociosController : ControllerBase
    {
        private readonly IRolesSociosService _service;
        private readonly ILogger<RolesSociosController> _logger;

        private const string MENSAJE_EXITO = "Éxito";

        public RolesSociosController(IRolesSociosService service, ILogger<RolesSociosController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<List<TipoSocioDTO>> GetPossibleRoles()
        {
            List<TipoSocioDTO> roles = null;
            try
            {
                roles = _service.GetPossibleRoles();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return Ok(roles);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "a,s")]
        public ActionResult<List<string>> GetRoles(int id)
        {
            List<string> roles = null;
            try
            {
                roles = _service.GetPartnerRoles(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }

            return Ok(roles);
        }

        [HttpPost("{id}")]
        [Authorize(Roles = "a,sa")]
        public ActionResult AssignRoles(int id, List<int> roles)
        {
            try
            {
                _service.AssignRoles(id, roles);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "a,sa")]
        public ActionResult UpdateRoles(int id, List<int> roles)
        {
            try
            {
                _service.UpdateRoles(id, roles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "a,sa")]
        public ActionResult DeleteRoles(int id)
        {
            try
            {
                _service.DeleteRoles(id);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return NoContent();
        }
    }
}
