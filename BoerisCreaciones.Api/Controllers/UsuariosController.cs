using BoerisCreaciones.Core.Models;
using BoerisCreaciones.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BoerisCreaciones.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuariosService _service;
        private readonly ILogger<UsuariosController> _logger;

        private const string MENSAJE_EXITO = "Éxito";

        public UsuariosController(IUsuariosService service, ILogger<UsuariosController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("Autenticar")]
        public ActionResult<MensajeSolicitud> Authenticate([FromBody] Usuario userObj)
        {
            if (userObj == null)
                return BadRequest();

            dynamic response;
            bool error = false;
            try
            {
                response = _service.Authenticate(userObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = ex.Message;
                error = true;
            }

            return Ok(new MensajeSolicitud(response, error));
        }

        [HttpPost("RegistrarUsuario")]
        public ActionResult<MensajeSolicitud> UserRegister([FromBody] Usuario userObj)
        {
            if(userObj == null)
                return BadRequest();

            string response = MENSAJE_EXITO;
            bool error = false;

            try
            {
                _service.RegisterUser(userObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = ex.Message;
                error = true;
            }

            return Ok(new MensajeSolicitud(response, error));
        }
    }
}
