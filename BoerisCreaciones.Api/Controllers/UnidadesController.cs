using BoerisCreaciones.Core.Models.Unidades;
using BoerisCreaciones.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace BoerisCreaciones.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnidadesController : Controller
    {
        private readonly IUnidadesService _unidadesService;

        public UnidadesController(IUnidadesService unidadesService, ILogger<UnidadesController> logger)
        {
            _unidadesService = unidadesService;
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
                Log.Error(ex.Message);
                return NotFound(ex.Message);
            }

            return Ok(units);
        }
    }
}
