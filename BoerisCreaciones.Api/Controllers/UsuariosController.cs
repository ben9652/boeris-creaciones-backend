﻿using AutoMapper;
using BoerisCreaciones.Core.Models;
using BoerisCreaciones.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Serilog;

namespace BoerisCreaciones.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuariosService _service;
        private readonly IRolesSociosService _partnersRolesService;
        private readonly IMapper _mapper;

        private const string MENSAJE_EXITO = "Éxito";

        public UsuariosController(IUsuariosService service, IRolesSociosService partnersRolesService, IMapper mapper)
        {
            _service = service;
            _partnersRolesService = partnersRolesService;
            _mapper = mapper;
        }

        [HttpGet("Testing")]
        public ActionResult<string> Test(string? optStr)
        {
            string message = "Bienvenido al controlador de usuarios";
            return Ok(message + (optStr != null ? $" {optStr}" : ""));
        }

        [HttpGet]
#if RELEASE
        [Authorize]
#endif
        public ActionResult<UsuarioDTO> GetAuthenticatedUser()
        {
            string stringId = User.FindFirst(ClaimTypes.SerialNumber)?.Value;
            int userId = Convert.ToInt32(stringId);
            UsuarioVM userDatabase = _service.GetUserById(Convert.ToInt32(userId));

            UsuarioDTO user = _mapper.Map<UsuarioDTO>(userDatabase);

            return Ok(user);
        }

        [HttpGet("ComprobarPassword")]
#if RELEASE
        [Authorize]
#endif
        public ActionResult<bool> CheckPassword(int id, string password)
        {
            try
            {
                _service.CheckPassword(id, password);
            }
            catch (Exception)
            {
                return Ok(false);
            }

            return Ok(true);
        }

        [HttpPost]
        public ActionResult<MensajeSolicitud> Login(UsuarioLogin credentials)
        {
            if (credentials.username == "" || credentials.password == "")
                return BadRequest(new MensajeSolicitud("Deben llenarse los 2 campos", true));

            bool error = false;
            dynamic response = MENSAJE_EXITO;
            UsuarioDTO? user = null;
            try
            {
                user = _service.Authenticate(credentials);
                if (user == null)
                    return NotFound(new MensajeSolicitud("No existe el usuario", true));

                if(user.role == 's')
                {
                    List<string> roles = _partnersRolesService.GetPartnerRoles(user.id_user);
                    response = _service.GenerateToken(user, roles);
                }
                else
                    response = _service.GenerateToken(user, null);

                Log.Information($"Usuario inició sesión: {user.username}");
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
                response = ex.Message;
                error = true;
            }

            return Ok(new MensajeSolicitud(response, error));
        }

        [HttpPost("Registrar")]
#if RELEASE
        [Authorize(Roles = "a")]
#endif
        public ActionResult<MensajeSolicitud> RegisterUser(UsuarioRegistro userObj)
        {
            if (userObj == null)
                return BadRequest();

            string response = MENSAJE_EXITO;
            bool error = false;

            try
            {
                _service.RegisterUser(userObj);
                Log.Information($"Usuario registrado: {userObj.username}");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                response = ex.Message;
                error = true;
            }

            return Ok(new MensajeSolicitud(response, error));
        }


        [HttpPatch("{id}")]
#if RELEASE
        [Authorize(Roles = "a,s")]
#endif
        public ActionResult UpdateUser(int id, JsonPatchDocument<UsuarioVM> patchDoc)
        {
            if (!IsUserAuthenticated(id))
                return BadRequest();

            UsuarioVM user = _service.GetUserById(id);
            if (user == null)
                return NotFound(new MensajeSolicitud("No existe el usuario", true));

            patchDoc.ApplyTo(user, ModelState);
            if (!TryValidateModel(user))
                return ValidationProblem(ModelState);

            if (patchDoc.Operations.Count == 0)
                return NoContent();

            List<string> attributes = new();
            foreach(Operation<UsuarioVM> ops in patchDoc.Operations)
                attributes.Add(ops.path);

            try
            {
                _service.UpdateUser(user, attributes);
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
                return Ok(new MensajeSolicitud(ex.Message, true));
            }

            UsuarioDTO userClient= _mapper.Map<UsuarioDTO>(user);

            return Ok(new MensajeSolicitud(userClient, false));
        }

        [HttpDelete("{id}")]
#if RELEASE
        [Authorize(Roles = "a")]
#endif
        public ActionResult DeleteUser(int id)
        {
            UsuarioVM user = _service.GetUserById(id);
            if (user == null)
                return NotFound(new MensajeSolicitud("No existe el usuario", true));

            _service.DeleteUser(id);
            Log.Information($"Usuario eliminado: {user.username}");

            return Ok(new MensajeSolicitud("Usuario eliminado con éxito", false));
        }

        private bool IsUserAuthenticated(int id)
        {
            var claimsOfUser = HttpContext.User.Identities.First().Claims;
            string serialNumber = claimsOfUser.First().Value;
            return id == Convert.ToInt32(serialNumber);
        }
    }
}
