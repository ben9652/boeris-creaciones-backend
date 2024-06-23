using BoerisCreaciones.Core.Models;
using BoerisCreaciones.Service.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
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

        [HttpGet("Autenticar")]
        public ActionResult<MensajeSolicitud> Authenticate(string username, string password)
        {
            if (username == "" || password == "")
                return BadRequest();

            dynamic response;
            bool error = false;
            try
            {
                response = _service.Authenticate(new UsuarioLogin(username, password));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = ex.Message;
                error = true;
            }

            return Ok(new MensajeSolicitud(response, error));
        }

        [HttpPost("Registrar")]
        public ActionResult<MensajeSolicitud> RegisterUser(UsuarioRegistro userObj)
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

        [HttpPatch("{id}")]
        public ActionResult UpdateUser(int id, JsonPatchDocument<UsuarioVM> patchDoc)
        {
            UsuarioVM user = _service.GetUserById(id);
            if (user == null)
                return NotFound(new MensajeSolicitud("No existe el usuario", true));

            patchDoc.ApplyTo(user, ModelState);
            if (!TryValidateModel(user))
                return ValidationProblem(ModelState);

            bool modifiedPassword = patchDoc.Operations.Find(op => 
                op.OperationType == OperationType.Replace &&
                op.path == "password"
            ) != null;

            _service.UpdateUser(user, modifiedPassword);

            return Ok(new MensajeSolicitud("Cambios realizados con éxito", false));
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteUser(int id)
        {
            UsuarioVM user = _service.GetUserById(id);
            if (user == null)
                return NotFound(new MensajeSolicitud("No existe el usuario", true));

            _service.DeleteUser(id);

            return Ok(new MensajeSolicitud("Usuario eliminado con éxito", false));
        }
    }
}
