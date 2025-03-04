using AutoMapper;
using BoerisCreaciones.Core.Models.Compras;
using BoerisCreaciones.Repository.Interfaces;
using BoerisCreaciones.Service.Interfaces;

namespace BoerisCreaciones.Service.Services
{
    public class ComprasService : IComprasService
    {
        private readonly IComprasRepository _repositoryCompras;
        private readonly ICatalogoProveedoresRepository _repositoryCatalogoProveedores;
        private readonly IUsuariosRepository _repositoryUsuarios;
        private readonly IRolesSociosService _rolesSociosService;
        private readonly IMapper _mapper;

        public ComprasService(IComprasRepository repositoryCompras, ICatalogoProveedoresRepository repositoryCatalogoProveedores, IUsuariosRepository repositoryUsuarios, IRolesSociosService rolesSociosService, IMapper mapper)
        {
            _repositoryCompras = repositoryCompras;
            _repositoryCatalogoProveedores = repositoryCatalogoProveedores;
            _repositoryUsuarios = repositoryUsuarios;
            _rolesSociosService = rolesSociosService;
            _mapper = mapper;
        }

        public List<CompraDTO> GetPurchases(int userId)
        {
            List<CompraVM> purchases = _repositoryCompras.GetPurchases();
            List<CompraDTO> purchasesDTO = new List<CompraDTO>();
            foreach (CompraVM purchase in purchases)
            {
                List<MateriaPrimaCompraVM> rawMaterials = _repositoryCompras.GetPurchasedRawMaterials(purchase.id_compra);
                CompraDTO purchaseDTO = _mapper.Map<CompraDTO>(purchase);
                purchaseDTO.raw_materials = new List<MateriaPrimaCompraDTO>();
                foreach (MateriaPrimaCompraVM rawMaterial in rawMaterials)
                {
                    MateriaPrimaCompraDTO rawMaterialDTO = _mapper.Map<MateriaPrimaCompraDTO>(rawMaterial);
                    purchaseDTO.raw_materials.Add(rawMaterialDTO);
                }
                purchasesDTO.Add(purchaseDTO);
            }


            List<string> roles = _rolesSociosService.GetPartnerRoles(userId);

            // Elimino cada compra que tiene de estado 'I' (inactivo) si el usuario es un surtidor
            if (roles.Contains("ss"))
                purchasesDTO.RemoveAll(p => p.status == 'I');

            return purchasesDTO;
        }

        public CompraDTO GetPurchaseById(int idPurchase, int userId)
        {
            CompraVM purchase = _repositoryCompras.GetPurchaseById(idPurchase);
            List<string> roles = _rolesSociosService.GetPartnerRoles(userId);

            // Si el usuario es un surtidor y la compra está inactiva, no la devuelvo
            if (roles.Contains("ss") && purchase.estado == 'I')
                return null;

            List<MateriaPrimaCompraVM> rawMaterialPurchase = _repositoryCompras.GetPurchasedRawMaterials(idPurchase);
            CompraDTO purchaseDTO = _mapper.Map<CompraDTO>(purchase);
            purchaseDTO.raw_materials = new List<MateriaPrimaCompraDTO>();
            foreach (MateriaPrimaCompraVM rawMaterial in rawMaterialPurchase)
            {
                MateriaPrimaCompraDTO rawMaterialDTO = _mapper.Map<MateriaPrimaCompraDTO>(rawMaterial);
                purchaseDTO.raw_materials.Add(rawMaterialDTO);
            }

            return purchaseDTO;
        }

        public List<CompraDTO> GetPurchasesByPartner(int idPartner, int userId)
        {
            List<CompraVM> purchases = _repositoryCompras.GetPurchasesByPartner(idPartner);
            List<CompraDTO> purchasesDTO = new List<CompraDTO>();
            foreach (CompraVM purchase in purchases)
            {
                List<MateriaPrimaCompraVM> rawMaterials = _repositoryCompras.GetPurchasedRawMaterials(purchase.id_compra);
                CompraDTO purchaseDTO = _mapper.Map<CompraDTO>(purchase);
                purchaseDTO.raw_materials = new List<MateriaPrimaCompraDTO>();
                foreach (MateriaPrimaCompraVM rawMaterial in rawMaterials)
                {
                    MateriaPrimaCompraDTO rawMaterialDTO = _mapper.Map<MateriaPrimaCompraDTO>(rawMaterial);
                    purchaseDTO.raw_materials.Add(rawMaterialDTO);
                }
                purchasesDTO.Add(purchaseDTO);
            }

            List<string> roles = _rolesSociosService.GetPartnerRoles(userId);

            // Elimino cada compra que tiene de estado 'I' (inactivo) si el usuario es un surtidor
            if (roles.Contains("ss"))
                purchasesDTO.RemoveAll(p => p.status == 'I');

            return purchasesDTO;
        }

        public List<MateriaPrimaCompraDTO> GetPurchasedRawMaterials(int idPurchase, int userId)
        {
            List<string> roles = _rolesSociosService.GetPartnerRoles(userId);

            // Si el usuario es un surtidor y la compra está inactiva, no devuelvo las materias primas
            if (roles.Contains("ss") && _repositoryCompras.GetPurchaseById(idPurchase).estado == 'I')
                throw new Exception("La compra no existe");

            List<MateriaPrimaCompraVM> rawMaterials = _repositoryCompras.GetPurchasedRawMaterials(idPurchase);
            List<MateriaPrimaCompraDTO> rawMaterialsDTO = new List<MateriaPrimaCompraDTO>();
            foreach (MateriaPrimaCompraVM rawMaterial in rawMaterials)
            {
                MateriaPrimaCompraDTO rawMaterialDTO = _mapper.Map<MateriaPrimaCompraDTO>(rawMaterial);
                rawMaterialsDTO.Add(rawMaterialDTO);
            }

            return rawMaterialsDTO;
        }

        public CompraDTO AddPurchase(NuevaCompraDTO newPurchase)
        {
            CompraVM compraNueva = _repositoryCompras.AddPurchase(newPurchase);
            CompraDTO compraNuevaDTO = _mapper.Map<CompraDTO>(compraNueva);
            List<MateriaPrimaCompraVM> materiasPrimas = _repositoryCompras.GetPurchasedRawMaterials(compraNueva.id_compra);

            compraNuevaDTO.raw_materials = new List<MateriaPrimaCompraDTO>();
            foreach (MateriaPrimaCompraVM materiaPrima in materiasPrimas)
            {
                MateriaPrimaCompraDTO materiaPrimaDTO = _mapper.Map<MateriaPrimaCompraDTO>(materiaPrima);
                compraNuevaDTO.raw_materials.Add(materiaPrimaDTO);
            }

            return compraNuevaDTO;
        }

        public void ReceivePurchase(int idPurchase, int userId, int idBranch)
        {
            char estado = _repositoryCompras.GetPurchaseById(idPurchase).estado;
            if (estado == 'C')
                throw new Exception("La compra está ya cancelada, y no puede ser recibida");
            if (estado == 'R')
                throw new Exception("La compra ya fue recibida");
            if (estado == 'I')
                throw new Exception("La compra está inactiva, y no puede ser recibida");

            if (_repositoryCompras.GetPurchaseById(idPurchase) == null)
            {
                throw new Exception("La compra no existe");
            }

            _repositoryCompras.ReceivePurchase(idPurchase, userId, idBranch);
        }  

        public void CancelPurchase(int idPurchase)
        {
            char estado = _repositoryCompras.GetPurchaseById(idPurchase).estado;
            if (estado == 'C')
                throw new Exception("La compra ya fue cancelada");
            if (estado == 'R')
                throw new Exception("La compra ya fue recibida, y no puede ser cancelada");
            if (estado == 'I')
                throw new Exception("La compra está inactiva, y no puede ser cancelada");

            if (_repositoryCompras.GetPurchaseById(idPurchase) == null)
            {
                throw new Exception("La compra no existe");
            }

            _repositoryCompras.CancelPurchase(idPurchase);
        }

        public void DisablePurchase(int idPurchase)
        {
            if (_repositoryCompras.GetPurchaseById(idPurchase) == null)
            {
                throw new Exception("La compra no existe");
            }
            
            _repositoryCompras.DisablePurchase(idPurchase);
        }

        public void DeletePurchase(int idPurchase)
        {
            CompraVM compra = _repositoryCompras.GetPurchaseById(idPurchase);
            if (compra == null)
                return;

            if (compra.estado != 'I')
                throw new Exception("La compra no está inactiva, y no puede ser eliminada");

            _repositoryCompras.DeletePurchase(idPurchase);
        }
    }
}
