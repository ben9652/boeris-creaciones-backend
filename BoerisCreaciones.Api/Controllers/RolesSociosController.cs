using BoerisCreaciones.Core.Models.Socio;
using BoerisCreaciones.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace BoerisCreaciones.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesSociosController : ControllerBase
    {
        private readonly IRolesSociosService _service;

        private const string MENSAJE_EXITO = "Éxito";

        public RolesSociosController(IRolesSociosService service)
        {
            _service = service;
        }

        [HttpGet]
#if RELEASE
        [Authorize]
#endif
        public ActionResult<List<TipoSocioDTO>> GetPossibleRoles()
        {
            List<TipoSocioDTO> roles = null;
            try
            {
                roles = _service.GetPossibleRoles();
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
                return NotFound(new { ex.Message });
            }

            return Ok(roles);
        }

        [HttpGet("{id}")]
#if RELEASE
        [Authorize(Roles = "a,s")]
#endif
        public ActionResult<List<string>> GetRoles(int id)
        {
            List<string> roles = null;
            try
            {
                roles = _service.GetPartnerRoles(id);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return BadRequest(new { ex.Message });
            }

            return Ok(roles);
        }

        [HttpPost("{id}")]
#if RELEASE
        [Authorize(Roles = "a,sa")]
#endif
        public ActionResult AssignRoles(int id, List<int> roles)
        {
            try
            {
                _service.AssignRoles(id, roles);

                string rolesString = string.Join(", ", roles);
                Log.Information($"Roles {rolesString} asignados al socio con id {id}");
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
                return BadRequest(new { ex.Message });
            }

            return NoContent();
        }

        [HttpPut("{id}")]
#if RELEASE
        [Authorize(Roles = "a,sa")]
#endif
        public ActionResult UpdateRoles(int id, List<int> roles)
        {
            try
            {
                _service.UpdateRoles(id, roles);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return BadRequest(new { ex.Message });
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
#if RELEASE
        [Authorize(Roles = "a,sa")]
#endif
        public ActionResult DeleteRoles(int id)
        {
            try
            {
                _service.DeleteRoles(id);
                Log.Information($"Roles del socio con id {id} eliminados");
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
                return BadRequest(new { ex.Message });
            }

            return NoContent();
        }
    }
}
