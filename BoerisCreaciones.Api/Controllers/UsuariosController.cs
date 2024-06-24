using AutoMapper;
using BoerisCreaciones.Core.Models;
using BoerisCreaciones.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;

namespace BoerisCreaciones.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuariosService _service;
        private readonly ILogger<UsuariosController> _logger;
        private readonly IMapper _mapper;

        private const string MENSAJE_EXITO = "Éxito";

        public UsuariosController(IUsuariosService service, ILogger<UsuariosController> logger, IMapper mapper)
        {
            _service = service;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<UsuarioDTO> GetAuthenticatedUser()
        {
            string stringId = User.FindFirst(ClaimTypes.SerialNumber)?.Value;
            int userId = Convert.ToInt32(stringId);
            UsuarioVM userDatabase = _service.GetUserById(Convert.ToInt32(userId));

            UsuarioDTO user = _mapper.Map<UsuarioDTO>(userDatabase);

            return Ok(user);
        }

        [HttpPost]
        public ActionResult<MensajeSolicitud> Login(UsuarioLogin credentials)
        {
            if (credentials.username == "" || credentials.password == "")
                return BadRequest(new MensajeSolicitud("Deben llenarse los 2 campos", true));

            bool error = false;
            dynamic response = MENSAJE_EXITO;
            UsuarioDTO user = null;
            try
            {
                user = _service.Authenticate(credentials);
                if (user == null)
                    return NotFound(new MensajeSolicitud("No existe el usuario", true));

                var token = _service.GenerateToken(user);
                response = token;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                response = ex.Message;
                error = true;
            }

            return Ok(new MensajeSolicitud(response, error));
        }

        [HttpPost("Registrar")]
        [Authorize(Roles = "a")]
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
        [Authorize]
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
        [Authorize(Roles = "a")]
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
