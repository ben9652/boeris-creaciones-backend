using BoerisCreaciones.Core.Models.Compras;
using BoerisCreaciones.Core.Models.PrimeNG;

namespace BoerisCreaciones.Service.Interfaces
{
    public interface IComprasService
    {
        public List<CompraDTO> GetPurchases(int userId, List<char> filters, BusquedaCompra? search, string? sortKey, bool ascendingSort);
        public CompraDTO GetPurchaseById(int idPurchase, int userId);
        public List<CompraDTO> GetPurchasesByPartner(int idPartner, int userId);
        public List<MateriaPrimaCompraDTO> GetPurchasedRawMaterials(int idPurchase, int userId);
        public List<TreeNode<string>> GetSortNodes();
        public List<FiltroCompra> GetFilters();
        public List<BusquedaCompra> GetSearchFilters();
        public CompraDTO AddPurchase(NuevaCompra newPurchase);
        public CompraDTO ReceivePurchase(int idPurchase, int userId, RecepcionCompra purchaseReception);
        public CompraDTO CancelPurchase(int idPurchase);
        public void DisablePurchase(int idPurchase);
        public void DeletePurchase(int idPurchase);
    }
}
