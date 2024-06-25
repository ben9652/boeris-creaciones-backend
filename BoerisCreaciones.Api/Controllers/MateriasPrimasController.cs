using BoerisCreaciones.Core.Models;
using BoerisCreaciones.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BoerisCreaciones.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MateriasPrimasController : ControllerBase
    {
        private readonly IMateriasPrimasService _service;
        private readonly ILogger<MateriasPrimasController> _logger;

        private const string MENSAJE_EXITO = "Éxito";

        public MateriasPrimasController(IMateriasPrimasService service, ILogger<MateriasPrimasController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<MensajeSolicitud> ListRawMaterials()
        {
            dynamic response;
            bool error = false;
            try
            {
                response = _service.ListRawMaterials();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                response = ex.Message;
                error = true;
            }

            return Ok(new MensajeSolicitud(response, error));
        }
    }
}
