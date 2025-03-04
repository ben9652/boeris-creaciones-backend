using BoerisCreaciones.Core.Models;
using BoerisCreaciones.Core.Models.Compras;
using BoerisCreaciones.Core.Models.MateriasPrimas;
using BoerisCreaciones.Core.Models.PrimeNG;
using BoerisCreaciones.Core.Models.Proveedores;
using BoerisCreaciones.Service.Helpers;
using BoerisCreaciones.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BoerisCreaciones.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComprasController : Controller
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
                _env.WebRootPath = webRootPath;
            }
        }

        [HttpGet]
#if RELEASE
        [Authorize(Roles = "a,sa,ss")]
#endif
        public IActionResult GetCompras(int? userId, string? filters = null, string? searchKey = null, string? searchInput = null, string? sort = "1-0", bool ascendingSort = false)
        {
            List<CompraDTO> compras = new List<CompraDTO>();

            if (userId == null)
                return BadRequest(new { message = "El ID de usuario es requerido" });

            List<char> filtros = new List<char>();
            if (!string.IsNullOrEmpty(filters))
            {
                string[] keyFilters = filters.Split('-');
                foreach (string keyFilter in keyFilters)
                {
                    // Si keyFilter no es 'P', 'R', o 'C', devolver un BadRequest
                    if (keyFilter != "P" && keyFilter != "R" && keyFilter != "C")
                        return BadRequest(new { message = "Filtro no válido" });
                }
            }

            BusquedaCompra? busqueda = null;
            if (!string.IsNullOrEmpty(searchKey) && !string.IsNullOrEmpty(searchInput))
            {
                if (
                    searchKey != "id" &&
                    searchKey != "description" &&
                    searchKey != "partner" &&
                    searchKey != "provider" &&
                    searchKey != "date" &&
                    searchKey != "budget" &&
                    searchKey != "branch"
                )
                    return BadRequest(new { message = "Clave de búsqueda no válida" });

                busqueda = new BusquedaCompra(searchKey, searchInput);
            }

            if (!string.IsNullOrEmpty(sort))
            {
                if (
                    sort != "0-0" &&
                    sort != "0-1" &&
                    sort != "0-2" &&
                    sort != "1-0" &&
                    sort != "1-1" &&
                    sort != "1-2" &&
                    sort != "2-0" &&
                    sort != "2-1"
                )
                    return BadRequest(new { message = "Clave de ordenamiento no válido" });
            }

            try
            {
                compras = _comprasService.GetPurchases((int)userId, filtros, busqueda, sort, ascendingSort);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las compras");
                return BadRequest(new { message = ex.Message });
            }
            return Ok(compras);
        }

        [HttpGet("{id}")]
#if RELEASE
        [Authorize(Roles = "a,sa,ss")]
#endif
        public IActionResult GetCompra(int id, int? userId)
        {
            if (userId == null)
                return BadRequest(new { message = "El ID de usuario es requerido" });

            CompraDTO compra;
            try
            {
                compra = _comprasService.GetPurchaseById(id, (int)userId);
                if (compra == null)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la compra {id}", id);
                return BadRequest(new { message = ex.Message });
            }
            return Ok(compra);
        }

        [HttpGet("filtros")]
        public IActionResult GetFiltros()
        {
            List<FiltroCompra> filtros = _comprasService.GetFilters();
            return Ok(filtros);
        }

        [HttpGet("filtros-busqueda")]
        public IActionResult GetSearchFilters()
        {
            List<BusquedaCompra> filtrosBusqueda = _comprasService.GetSearchFilters();
            return Ok(filtrosBusqueda);
        }

        [HttpGet("ordenamiento")]
        public IActionResult GetArbolOrdenamiento()
        {
            List<TreeNode<string>> treeNodes = _comprasService.GetSortNodes();
            
            return Ok(new { tree = treeNodes, initialIndex = "1-0" });
        }

        [HttpPost]
#if RELEASE
        [Authorize(Roles = "a,sa,ss")]
#endif
        public IActionResult PostCompra(NuevaCompra compra)
        {
            if(compra.raw_materials == null || compra.raw_materials.Count == 0)
                return BadRequest(new { message = "La compra debe tener al menos una materia prima" });
            UsuarioVM socio = _sociosService.GetUserById(compra.partner.id_user);
            if (socio == null || (socio != null && socio.rol != 's'))
                return NotFound(new { message = "El usuario que solicitó la compra no es un socio" });

            ProveedorDTO proveedor = _proveedoresService.GetProvider(compra.provider.id);
            if (proveedor == null)
                return NotFound(new { message = "Proveedor no encontrado" });

            // Compruebo si todas las materias primas de la lista son del mismo rubro que el proveedor
            List<MateriaPrimaDTO> materiasPrimas = _materiasPrimasService.GetRawMaterialsItems();
            foreach (MateriaPrimaCompraDTO materiaPrima in compra.raw_materials)
            {
                MateriaPrimaDTO? materiaPrimaDTO = materiasPrimas.Find(m => m.id == materiaPrima.raw_material_id);
                if (materiaPrimaDTO == null)
                    return NotFound(new { message = $"La materia prima {materiaPrima.raw_material_id} no existe" });

                if (materiaPrimaDTO.category.id != proveedor.category.id)
                    return BadRequest(new { message = $"La materia prima {materiaPrima.raw_material_id} no pertenece al rubro del proveedor {proveedor.id}" });
            }

            CompraDTO compraNueva = null;

            try
            {
                compraNueva = _comprasService.AddPurchase(compra);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar la compra");
                return BadRequest(new { message = ex.Message });
            }

            return Ok(compraNueva);
        }

        [HttpPost("recibir/{id}-{userId}")]
#if RELEASE
        [Authorize(Roles = "a,sa,ss")]
#endif
        public IActionResult RecibirCompra(int id, int? userId, RecepcionCompra purchaseReception)
        {
            if (userId == null)
                return BadRequest(new { message = "El ID de usuario es requerido" });

            CompraDTO purchase = null;

            try
            {
                purchase = _comprasService.ReceivePurchase(id, (int)userId, purchaseReception);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recibir la compra {id}", id);
                return BadRequest(new { message = ex.Message });
            }

            return Ok(purchase);
        }

        [HttpPost("cancelar/{id}-{userId}")]
#if RELEASE
        [Authorize(Roles = "a,sa,ss")]
#endif
        public IActionResult CancelarCompra(int id)
        {
            CompraDTO compra = null;

            try
            {
                compra = _comprasService.CancelPurchase(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cancelar la compra {id}", id);
                return BadRequest(new { message = ex.Message });
            }

            return Ok(compra);
        }

        [HttpPost("dar-de-baja/{id}")]
#if RELEASE
        [Authorize(Roles = "a,sa")]
#endif
        public IActionResult DarDeBajaCompra(int id)
        {
            try
            {
                _comprasService.DisablePurchase(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al dar de baja la compra {id}", id);
                return BadRequest(new { message = ex.Message });
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
#if RELEASE
        [Authorize(Roles = "a,sa")]
#endif
        public IActionResult EliminarCompra(int id)
        {
            try
            {
                _comprasService.DeletePurchase(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la compra {id}", id);
                return BadRequest(new { message = ex.Message });
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
                string fileName = await MultimediaManaging.UploadFile(files[0], _env.WebRootPath, controllerName);

                // Devolver la URL o la ruta del archivo guardado
                url = $"https://{Request.Host}:9354/{controllerName}/{fileName}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { message = ex.Message });
            }

            return Ok(url);
        }
    }
}
