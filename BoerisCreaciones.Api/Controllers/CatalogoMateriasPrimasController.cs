﻿using BoerisCreaciones.Core.Models.MateriasPrimas;
using BoerisCreaciones.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using BoerisCreaciones.Service.Helpers;

namespace BoerisCreaciones.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogoMateriasPrimasController : Controller
    {
        private readonly ICatalogoMateriasPrimasService _service;
        private readonly ILogger<CatalogoMateriasPrimasController> _logger;
        private readonly IWebHostEnvironment _env;

        public CatalogoMateriasPrimasController(ICatalogoMateriasPrimasService service, ILogger<CatalogoMateriasPrimasController> logger, IWebHostEnvironment env)
        {
            _service = service;
            _logger = logger;
            _env = env;

            var webRootPath = _env.WebRootPath;
            if (string.IsNullOrEmpty(webRootPath))
            {
                webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }
        }

        [HttpGet]
#if RELEASE
        [Authorize(Roles = "a,sa")]
#endif
        public ActionResult<List<MateriasPrimasItemDTO>> Get()
        {
            List<MateriasPrimasItemDTO> catalogoMateriasPrimas = new List<MateriasPrimasItemDTO>();

            try
            {
                catalogoMateriasPrimas = _service.GetRawMaterialsItems();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }

            return Ok(catalogoMateriasPrimas);
        }

        [HttpPost]
#if RELEASE
        [Authorize(Roles = "a,sa")]
#endif
        public ActionResult<MateriasPrimasItemDTO> Create(MateriasPrimasItemDTO itemMateriaPrima)
        {
            char origen = itemMateriaPrima.source;
            if (origen != 'C' && origen != 'E' && origen != 'P')
                return BadRequest("El valor del origen es inválido");

            try
            {
                itemMateriaPrima = _service.CreateRawMaterialItem(itemMateriaPrima);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }

            return Ok(itemMateriaPrima);
        }

        [HttpPost("upload-image")]
#if RELEASE
        [Authorize(Roles = "a,sa")]
#endif
        public async Task<IActionResult> UploadImage()
        {
            IFormFileCollection files = Request.Form.Files;

            string url;
            string controllerName = "CatalogoMateriasPrimas";

            try
            {
                string fileName = await MultimediaManging.UploadImage(files[0], _env.WebRootPath, controllerName);

                // Devolver la URL o la ruta del archivo guardado
                url = $"{Request.Scheme}://{Request.Host}/{controllerName}/{fileName}";
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }

            return Ok(url);
        }

        [HttpDelete("delete-image/{imagePath}")]
#if RELEASE
        [Authorize(Roles = "a,sa")]
#endif
        public IActionResult DeleteImage(string imagePath)
        {
            string controllerName = "CatalogoMateriasPrimas";

            bool result = MultimediaManging.DeleteImage(imagePath, _env.WebRootPath, controllerName);

            return Ok(result);
        }

        [HttpPatch("{id}")]
#if RELEASE
        [Authorize(Roles = "a,sa")]
#endif
        public ActionResult Update(int id, JsonPatchDocument<MateriasPrimasItemDTO> patchDoc)
        {
            MateriasPrimasItemDTO item = _service.GetRawMaterialsItem(id);
            if (item == null)
                return NotFound("No existe la materia prima especificada");

            patchDoc.ApplyTo(item, ModelState);
            if (!TryValidateModel(item))
                return ValidationProblem(ModelState);

            if (patchDoc.Operations.Count == 0)
                return NoContent();

            List<string> attributes = new();
            foreach (Operation<MateriasPrimasItemDTO> ops in patchDoc.Operations)
                attributes.Add(ops.path);

            try
            {
                item = _service.UpdateRawMaterialItem(item, attributes);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }

            return Ok(item);
        }

        [HttpDelete("{id}")]
#if RELEASE
        [Authorize(Roles = "a,sa")]
#endif
        public ActionResult Delete(int id)
        {
            try
            {
                _service.DeleteRawMaterialItem(id);
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
