using BoerisCreaciones.Core.Models;
using BoerisCreaciones.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

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

        [HttpGet]
        public ActionResult<string> Testing(string? optMsg)
        {
            string message = "Bienvenido al controlador de Usuarios";

            return optMsg == null ? message : message + " " + optMsg;
        }

        [HttpPost("Autenticar")]
        public ActionResult<MensajeSolicitud> Authenticate([FromBody] UsuarioLogin userObj)
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
        public ActionResult<MensajeSolicitud> RegisterUser([FromBody] UsuarioRegistro userObj)
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
