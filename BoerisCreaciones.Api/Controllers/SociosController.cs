using AutoMapper;
using BoerisCreaciones.Core.Models;
using BoerisCreaciones.Core.Models.Socio;
using BoerisCreaciones.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace BoerisCreaciones.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SociosController : ControllerBase
    {
        private readonly ISociosService _service;
        private readonly ILogger<SociosController> _logger;

        private const string MENSAJE_EXITO = "Éxito";

        public SociosController(ISociosService service, ILogger<SociosController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
#if RELEASE
        [Authorize(Roles = "a,sa")]
#endif
        public ActionResult<List<SocioDTO>> GetPartners()
        {
            List<SocioDTO> partners = null;
            try
            {
                partners = _service.GetPartners();
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
                return NotFound(new { ex.Message });
            }

            return Ok(partners);
        }

        [HttpGet("{id}")]
#if RELEASE
        [Authorize]
#endif
        public ActionResult<SocioDTO> GetPartner(int id)
        {
            SocioDTO partner = null;
            try
            {
                partner = _service.GetPartner(id);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return NotFound(new { ex.Message });
            }
            return Ok(partner);
        }

        [HttpPost]
#if RELEASE
        [Authorize(Roles = "a,sa")]
#endif
        public ActionResult<SocioDTO> RegisterPartner(SocioRegistro partnerObj)
        {
            if (partnerObj == null)
                return BadRequest();

            SocioDTO partner = null;
            try
            {
                partner = _service.RegisterPartner(partnerObj);
                Log.Information($"Socio registrado: {partner.firstName} {partner.lastName} ({partner.id_user})");
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
                return StatusCode(412, new { ex.Message });
            }

            return Ok(partner);
        }
        
        [HttpDelete("{id}")]
#if RELEASE
        [Authorize(Roles = "a,sa")]
#endif
        public ActionResult DeletePartner(int id)
        {
            try
            {
                _service.DeletePartner(id);
                Log.Information($"Socio eliminado: {id}");
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
                return StatusCode(412, new { ex.Message });
            }

            return NoContent();
        }
    }
}
