using BoerisCreaciones.Core.Models.MateriasPrimas;
using BoerisCreaciones.Service.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using BoerisCreaciones.Service.Helpers;
using BoerisCreaciones.Core.Models.PrimeNG.Dropdown;
using BoerisCreaciones.Core.Models.Rubros;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;
using Serilog;

namespace BoerisCreaciones.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogoMateriasPrimasController : Controller
    {
        private readonly ICatalogoMateriasPrimasService _service;
        private readonly IWebHostEnvironment _env;

        public CatalogoMateriasPrimasController(ICatalogoMateriasPrimasService service, IWebHostEnvironment env)
        {
            _service = service;
            _env = env;

            var webRootPath = _env.WebRootPath;
            if (string.IsNullOrEmpty(webRootPath))
            {
                webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                _env.WebRootPath = webRootPath;
            }
        }

        [HttpGet]
#if RELEASE
        [Authorize(Roles = "a,sa")]
#endif
        public ActionResult<List<MateriaPrimaDTO>> Get()
        {
            List<MateriaPrimaDTO> catalogoMateriasPrimas = new List<MateriaPrimaDTO>();

            try
            {
                catalogoMateriasPrimas = _service.GetRawMaterialsItems();
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
                return BadRequest(new { ex.Message });
            }

            return Ok(catalogoMateriasPrimas);
        }

        [HttpGet("Dropdown")]
#if RELEASE
        [Authorize]
#endif
        public ActionResult GetGroupedDropdown()
        {
            List<SelectItemGroup<RubroMateriaPrimaDTO, MateriaPrimaDTO>> dropdownAgrupado = new();

            try
            {
                dropdownAgrupado = _service.GetGroupedDropdown();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return BadRequest(new { ex.Message });
            }

            return Ok(dropdownAgrupado);
        }

        [HttpGet("Dropdown/{categories}")]
#if RELEASE
        [Authorize]
#endif
        public ActionResult GetGroupedDropdownWithFilters(string categories)
        {
            // Si `categories` no es una serie de números enteros separados por guiones, devolver un BadRequest
            if (!Regex.IsMatch(categories, @"^(\d+-)*\d+$"))
                return BadRequest(new { Message = "El formato de las categorías es inválido" });

            List<int> categoriesList = new();
            if (!string.IsNullOrEmpty(categories))
            {
                string[] categoriesArray = categories.Split('-');
                foreach (string category in categoriesArray)
                    categoriesList.Add(int.Parse(category));
            }

            List<SelectItemGroup<RubroMateriaPrimaDTO, MateriaPrimaDTO>> dropdownAgrupado = new();

            try
            {
                dropdownAgrupado = _service.GetGroupedDropdown(categoriesList);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return BadRequest(new { ex.Message });
            }

            return Ok(dropdownAgrupado);
        }

        [HttpPost]
#if RELEASE
        [Authorize(Roles = "a,sa")]
#endif
        public ActionResult<MateriaPrimaDTO> Create(MateriaPrimaDTO itemMateriaPrima)
        {
            char origen = itemMateriaPrima.source;
            if (origen != 'C' && origen != 'E' && origen != 'P')
                return BadRequest(new { Message = "El valor del origen es inválido" });

            try
            {
                itemMateriaPrima = _service.CreateRawMaterialItem(itemMateriaPrima);
                Log.Information($"Materia prima creada: {itemMateriaPrima.id}");
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
                return BadRequest(new { ex.Message });
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
                string fileName = await MultimediaManaging.UploadImage(files[0], _env.WebRootPath, controllerName);

                // Devolver la URL o la ruta del archivo guardado
                url = $"https://{Request.Host}:9354/{controllerName}/{fileName}";
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.Message);
                return BadRequest(new { ex.Message });
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

            bool result = MultimediaManaging.DeleteImage(imagePath, _env.WebRootPath, controllerName);

            return Ok(result);
        }

        [HttpPatch("{id}")]
#if RELEASE
        [Authorize(Roles = "a,sa")]
#endif
        public ActionResult Update(int id, JsonPatchDocument<MateriaPrimaDTO> patchDoc)
        {
            MateriaPrimaDTO item = _service.GetRawMaterialsItem(id);
            if (item == null)
                return NotFound("No existe la materia prima especificada");

            patchDoc.ApplyTo(item, ModelState);
            if (!TryValidateModel(item))
                return ValidationProblem(ModelState);

            if (patchDoc.Operations.Count == 0)
                return NoContent();

            List<string> attributes = new();
            foreach (Operation<MateriaPrimaDTO> ops in patchDoc.Operations)
                attributes.Add(ops.path);

            try
            {
                item = _service.UpdateRawMaterialItem(item, attributes);
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
                return BadRequest(new { ex.Message });
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
                Log.Information($"Materia prima eliminada: {id}");
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
                return StatusCode(412, new { ex.Message });
            }

            return NoContent();
        }
    }
}
