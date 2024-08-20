using BoerisCreaciones.Core.Models.Unidades;
using BoerisCreaciones.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BoerisCreaciones.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnidadesController : Controller
    {
        private readonly IUnidadesService _unidadesService;
        private readonly ILogger<UnidadesController> _logger;

        public UnidadesController(IUnidadesService unidadesService, ILogger<UnidadesController> logger)
        {
            _unidadesService = unidadesService;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult GetUnits()
        {
            List<UnidadDTO> units = new List<UnidadDTO>();
            try
            {
                units = _unidadesService.GetUnits();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }

            return Ok(units);
        }
    }
}
