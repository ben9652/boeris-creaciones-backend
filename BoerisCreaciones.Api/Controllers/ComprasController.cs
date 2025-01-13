using BoerisCreaciones.Core.Models;
using BoerisCreaciones.Core.Models.Compras;
using BoerisCreaciones.Core.Models.MateriasPrimas;
using BoerisCreaciones.Core.Models.Proveedores;
using BoerisCreaciones.Service;
using BoerisCreaciones.Service.Helpers;
using BoerisCreaciones.Service.Interfaces;
using BoerisCreaciones.Service.Services;
using DotNetEnv;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BoerisCreaciones.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComprasController : ControllerBase
    {
        private readonly IComprasService _comprasService;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ComprasController> _logger;
        private readonly ICatalogoProveedoresService _proveedoresService;
        private readonly IUsuariosService _sociosService;
        private readonly ICatalogoMateriasPrimasService _materiasPrimasService;

        public ComprasController(IComprasService comprasService, IWebHostEnvironment env, ILogger<ComprasController> logger, ICatalogoProveedoresService proveedoresService, IUsuariosService sociosService, ICatalogoMateriasPrimasService materiasPrimasService)
        {
            _comprasService = comprasService;
            _env = env;
            _logger = logger;
            _proveedoresService = proveedoresService;
            _sociosService = sociosService;
            _materiasPrimasService = materiasPrimasService;

            var webRootPath = _env.WebRootPath;
            if (string.IsNullOrEmpty(webRootPath))
            {
                webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }
        }

#if RELEASE
        [Authorize(Roles = "a,sa,ss")]
#endif
        [HttpGet]
        public IActionResult GetCompras(int userId)
        {
            List<CompraDTO> compras = new List<CompraDTO>();
            try
            {
                compras = _comprasService.GetPurchases(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las compras");
                return BadRequest(ex.Message);
            }
            return Ok(compras);
        }

#if RELEASE
        [Authorize(Roles = "a,sa,ss")]
#endif
        [HttpGet("{id}")]
        public IActionResult GetCompra(int id, int userId)
        {
            CompraDTO compra;
            try
            {
                compra = _comprasService.GetPurchaseById(id, userId);
                if (compra == null)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la compra {id}", id);
                return BadRequest(ex.Message);
            }
            return Ok(compra);
        }

#if RELEASE
        [Authorize(Roles = "a,sa,ss")]
#endif
        [HttpPost]
        public IActionResult PostCompra(NuevaCompraDTO compra)
        {
            if(compra.raw_materials == null || compra.raw_materials.Count == 0)
                return BadRequest("La compra debe tener al menos una materia prima");
            UsuarioVM socio = _sociosService.GetUserById(compra.partner.id_user);
            if (socio == null)
                return NotFound("Socio no encontrado");

            ProveedorDTO proveedor = _proveedoresService.GetProvider(compra.provider.id);
            if (proveedor == null)
                return NotFound("Proveedor no encontrado");

            // Compruebo si todas las materias primas de la lista son del mismo rubro que el proveedor
            List<MateriaPrimaDTO> materiasPrimas = _materiasPrimasService.GetRawMaterialsItems();
            foreach (MateriaPrimaCompraDTO materiaPrima in compra.raw_materials)
            {
                MateriaPrimaDTO? materiaPrimaDTO = materiasPrimas.Find(m => m.id == materiaPrima.raw_material_id);
                if (materiaPrimaDTO == null)
                    return NotFound($"La materia prima {materiaPrima.raw_material_id} no existe");

                if (materiaPrimaDTO.category.id != proveedor.category.id)
                    return BadRequest($"La materia prima {materiaPrima.raw_material_id} no pertenece al rubro del proveedor {proveedor.id}");
            }

            CompraDTO compraNueva = null;

            try
            {
                compraNueva = _comprasService.AddPurchase(compra);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar la compra");
                return BadRequest(ex.Message);
            }

            return Ok(compraNueva);
        }

#if RELEASE
        [Authorize(Roles = "a,ss")]
#endif
        [HttpPost("{id}/recibir")]
        public IActionResult RecibirCompra(int id, int userId)
        {
            try
            {
                _comprasService.ReceivePurchase(id, userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recibir la compra {id}", id);
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

#if RELEASE
        [Authorize(Roles = "a,ss")]
#endif
        [HttpPost("{id}/cancelar")]
        public IActionResult CancelarCompra(int id)
        {
            try
            {
                _comprasService.CancelPurchase(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cancelar la compra {id}", id);
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

#if RELEASE
        [Authorize(Roles = "a,sa")]
#endif
        [HttpPost("{id}/dar-de-baja")]
        public IActionResult DarDeBajaCompra(int id)
        {
            try
            {
                _comprasService.DisablePurchase(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al dar de baja la compra {id}", id);
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

#if RELEASE
        [Authorize(Roles = "a,sa")]
#endif
        [HttpDelete("{id}")]
        public IActionResult EliminarCompra(int id)
        {
            try
            {
                _comprasService.DeletePurchase(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la compra {id}", id);
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        [HttpPost("upload-file")]
#if RELEASE
        [Authorize(Roles = "a,sa,ss")]
#endif
        public async Task<IActionResult> UploadInvoice()
        {
            IFormFileCollection files = Request.Form.Files;

            string url;
            string controllerName = "Compras";

            try
            {
                string fileName = await MultimediaManaging.UploadImage(files[0], _env.WebRootPath, controllerName);

                // Devolver la URL o la ruta del archivo guardado
                url = $"{Request.Scheme}://{Request.Host}/{controllerName}/{fileName}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { ex.Message });
            }

            return Ok(url);
        }
    }
}
