using AutoMapper;
using BoerisCreaciones.Core.Models;
using BoerisCreaciones.Core.Models.Socio;
using BoerisCreaciones.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        [Authorize(Roles = "a,sa")]
        public ActionResult<List<SocioDTO>> GetPartners()
        {
            List<SocioDTO> partners = null;
            try
            {
                partners = _service.GetPartners();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return Ok(partners);
        }

        [HttpPost]
        [Authorize(Roles = "a,sa")]
        public ActionResult<SocioDTO> RegisterPartner(SocioRegistro partnerObj)
        {
            if (partnerObj == null)
                return BadRequest();

            SocioDTO partner = null;
            try
            {
                partner = _service.RegisterPartner(partnerObj);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return Ok(partner);
        }
        
        [HttpDelete("{id}")]
        [Authorize(Roles = "a,sa")]
        public ActionResult DeletePartner(int id)
        {
            try
            {
                _service.DeletePartner(id);
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
