﻿using BoerisCreaciones.Core.Models.Provincias;
using BoerisCreaciones.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;

namespace BoerisCreaciones.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvinciasController : Controller
    {
        private readonly IProvinciasService _service;
        private readonly ILogger<ProvinciasController> _logger;
        
        public ProvinciasController(IProvinciasService service, ILogger<ProvinciasController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
#if RELEASE
        [Authorize(Roles = "a")]
#endif
        public ActionResult GetAllProvinces()
        {
            List<ProvinciaDTO> provincias = new List<ProvinciaDTO>();

            try
            {
                provincias = _service.GetAllProvinces();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(new { ex.Message });
            }

            return Ok(provincias);
        }

        [HttpGet("Expanded")]
#if RELEASE
        [Authorize(Roles = "a")]
#endif
        public ActionResult GetAllProvincesFull()
        {
            List<ProvinciaExpandedDTO> provincias = new List<ProvinciaExpandedDTO>();

            try
            {
                provincias = _service.GetAllProvincesWithLocalities();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(new { ex.Message });
            }

            return Ok(provincias);
        }

        [HttpPost]
#if RELEASE
        [Authorize(Roles = "a")]
#endif
        public ActionResult CreateProvince(ProvinciaDTO provincia)
        {
            try
            {
                provincia = _service.CreateProvince(provincia);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { ex.Message });
            }

            return Ok(provincia);
        }

        [HttpPatch("{id}")]
#if RELEASE
        [Authorize(Roles = "a")]
#endif
        public ActionResult UpdateProvince(int id, JsonPatchDocument<ProvinciaDTO> patchDoc)
        {
            ProvinciaDTO provincia = _service.GetProvince(id);
            if (provincia == null)
                return NotFound("No existe la provincia especificada");

            patchDoc.ApplyTo(provincia, ModelState);
            if (!TryValidateModel(provincia))
                return ValidationProblem(ModelState);

            if (patchDoc.Operations.Count == 0)
                return NoContent();

            List<string> attributes = new();
            foreach (Operation<ProvinciaDTO> ops in patchDoc.Operations)
                attributes.Add(ops.path);

            try
            {
                provincia = _service.UpdateProvince(provincia, attributes);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { ex.Message });
            }

            return Ok(provincia);
        }

        [HttpDelete("{id}")]
#if RELEASE
        [Authorize(Roles = "a")]
#endif
        public ActionResult DeleteProvince(int id)
        {
            try
            {
                _service.DeleteProvince(id);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(412, new { ex.Message });
            }

            return NoContent();
        }
    }
}
