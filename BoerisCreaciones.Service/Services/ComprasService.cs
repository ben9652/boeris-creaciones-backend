﻿using AutoMapper;
using BoerisCreaciones.Core.Models;
using BoerisCreaciones.Core.Models.Compras;
using BoerisCreaciones.Core.Models.PrimeNG;
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

        private string GetDatabaseAttributeName(string sortKey)
        {
            string attributeName;

            if (!System.Text.RegularExpressions.Regex.IsMatch(sortKey, @"^\d+-\d+$"))
            {
                attributeName = sortKey switch
                {
                    "id" => "id_compra",
                    "description" => "descripcion",
                    "provider" => "proveedor",
                    "requester_partner" => "socio",
                    "date" => "fecha",
                    "order_date" => "fecha_pedido",
                    "reception_date" => "fecha_recepcion",
                    "canceled_date" => "fecha_cancelacion",
                    "budget" => "presupuesto",
                    "branch" => "sucursal",
                    _ => sortKey
                };
            }
            else
            {
                attributeName = sortKey switch
                {
                    "0-0" => "proveedor",
                    "0-1" => "socio",
                    "0-2" => "sucursal",
                    "1-0" => "fecha_pedido",
                    "1-1" => "fecha_recepcion",
                    "1-2" => "fecha_cancelacion",
                    "2-0" => "presupuesto",
                    "2-1" => "id_compra",
                    _ => sortKey
                };
            }

            return attributeName;
        }

        public List<CompraDTO> GetPurchases(int userId, List<char> filters, BusquedaCompra? search, string? sortKey, bool ascendingSort)
        {
            string? orderCriteria = sortKey != null ? GetDatabaseAttributeName(sortKey) : null;
            if (search != null)
                search.key = GetDatabaseAttributeName(search.key);

            List<CompraVM> purchases = _repositoryCompras.GetPurchases(filters, search, orderCriteria, ascendingSort);
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

            UsuarioVM user = _repositoryUsuarios.GetUserById(userId);
            if (user.id_usuario != 1)
            {
                List<string> roles = _rolesSociosService.GetPartnerRoles(userId);

                // Elimino cada compra que tiene de estado 'I' (inactivo) si el usuario es un surtidor
                if (roles.Contains("ss") && !roles.Contains("sa"))
                    purchasesDTO.RemoveAll(p => p.state == 'I');
            }

            return purchasesDTO;
        }

        public CompraDTO GetPurchaseById(int idPurchase, int userId)
        {
            CompraVM purchase = _repositoryCompras.GetPurchaseById(idPurchase);

            UsuarioVM user = _repositoryUsuarios.GetUserById(userId);
            if(user.id_usuario != 1)
            { 
                List<string> roles = _rolesSociosService.GetPartnerRoles(userId);

                // Si el usuario es un surtidor y la compra está inactiva, no la devuelvo
                if (roles.Contains("ss") && !roles.Contains("sa") && purchase.estado == 'I')
                    return null;
            }

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

            UsuarioVM user = _repositoryUsuarios.GetUserById(userId);
            if (user.id_usuario != 1)
            {
                List<string> roles = _rolesSociosService.GetPartnerRoles(userId);

                // Elimino cada compra que tiene de estado 'I' (inactivo) si el usuario es un surtidor
                if (roles.Contains("ss") && !roles.Contains("sa"))
                    purchasesDTO.RemoveAll(p => p.state == 'I');
            }

            return purchasesDTO;
        }

        public List<MateriaPrimaCompraDTO> GetPurchasedRawMaterials(int idPurchase, int userId)
        {
            UsuarioVM user = _repositoryUsuarios.GetUserById(userId);
            if (user.id_usuario != 1)
            {
                List<string> roles = _rolesSociosService.GetPartnerRoles(userId);

                // Si el usuario es un surtidor y la compra está inactiva, no devuelvo las materias primas
                if (roles.Contains("ss") && !roles.Contains("sa") && _repositoryCompras.GetPurchaseById(idPurchase).estado == 'I')
                    return null;
            }

            List<MateriaPrimaCompraVM> rawMaterials = _repositoryCompras.GetPurchasedRawMaterials(idPurchase);
            List<MateriaPrimaCompraDTO> rawMaterialsDTO = new List<MateriaPrimaCompraDTO>();
            foreach (MateriaPrimaCompraVM rawMaterial in rawMaterials)
            {
                MateriaPrimaCompraDTO rawMaterialDTO = _mapper.Map<MateriaPrimaCompraDTO>(rawMaterial);
                rawMaterialsDTO.Add(rawMaterialDTO);
            }

            return rawMaterialsDTO;
        }

        public List<TreeNode<string>> GetSortNodes()
        {
            List<TreeNode<string>> treeNodes = new List<TreeNode<string>>();

            TreeNode<string> alpha = new TreeNode<string>("0", "Alfabéticamente", "fas fa-font", "1", false);
            TreeNode<string> date = new TreeNode<string>("1", "Por fecha", "fas fa-calendar", "2", false);
            TreeNode<string> numbers = new TreeNode<string>("2", "Por número", "fas fa-hashtag", "3", false);

            alpha.children.Add(new TreeNode<string>("0-0", "Proveedores", "fas fa-user-tie", "1", "provider.name", new TreeNode<string>(alpha.key, alpha.label, alpha.icon, alpha.type, false)));
            alpha.children.Add(new TreeNode<string>("0-1", "Socios", "fas fa-user", "1", "requester_partner.firstName", new TreeNode<string>(alpha.key, alpha.label, alpha.icon, alpha.type, false)));
            alpha.children.Add(new TreeNode<string>("0-2", "Sucursal", "fas fa-store", "1", "branch.name", new TreeNode<string>(alpha.key, alpha.label, alpha.icon, alpha.type, false)));

            date.children.Add(new TreeNode<string>("1-0", "Pedido", "fas fa-calendar-check", "2", "order_date", new TreeNode<string>(date.key, date.label, date.icon, date.type, false)));
            date.children.Add(new TreeNode<string>("1-1", "Recepción", "fas fa-calendar-plus", "2", "reception_date", new TreeNode<string>(date.key, date.label, date.icon, date.type, false)));
            date.children.Add(new TreeNode<string>("1-2", "Cancelación", "fas fa-calendar-minus", "2", "canceled_date", new TreeNode<string>(date.key, date.label, date.icon, date.type, false)));

            numbers.children.Add(new TreeNode<string>("2-0", "Costo", "fas fa-dollar-sign", "3", "price", new TreeNode<string>(numbers.key, numbers.label, numbers.icon, numbers.type, false)));
            numbers.children.Add(new TreeNode<string>("2-1", "ID", "fas fa-barcode", "3", "id", new TreeNode<string>(numbers.key, numbers.label, numbers.icon, numbers.type, false)));

            treeNodes.Add(alpha);
            treeNodes.Add(date);
            treeNodes.Add(numbers);

            return treeNodes;
        }

        public List<FiltroCompra> GetFilters()
        {
            FiltroCompra pedidas = new FiltroCompra('P', "Pedidas", "yellow");
            FiltroCompra recibidas = new FiltroCompra('R', "Recibidas", "green");
            FiltroCompra canceladas = new FiltroCompra('C', "Canceladas", "red");

            return new List<FiltroCompra> { pedidas, recibidas, canceladas };
        }

        public List<BusquedaCompra> GetSearchFilters()
        {
            List<BusquedaCompra> searchFilters = new List<BusquedaCompra>();
            searchFilters.Add(new BusquedaCompra("id", "ID"));
            searchFilters.Add(new BusquedaCompra("description", "Descripción"));
            searchFilters.Add(new BusquedaCompra("requester_partner", "Socio"));
            searchFilters.Add(new BusquedaCompra("provider", "Proveedor"));
            searchFilters.Add(new BusquedaCompra("order_date", "Fecha"));
            searchFilters.Add(new BusquedaCompra("budget", "Presupuesto"));
            searchFilters.Add(new BusquedaCompra("reception_branch", "Sucursal de recepción"));

            return searchFilters;
        }

        public CompraDTO AddPurchase(NuevaCompra newPurchase)
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

        public CompraDTO ReceivePurchase(int idPurchase, int userId, RecepcionCompra purchaseReception)
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

            _repositoryCompras.ReceivePurchase(idPurchase, userId, purchaseReception);

            return GetPurchaseById(idPurchase, userId);
        }

        public CompraDTO CancelPurchase(int idPurchase)
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

            return GetPurchaseById(idPurchase, 1);
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
