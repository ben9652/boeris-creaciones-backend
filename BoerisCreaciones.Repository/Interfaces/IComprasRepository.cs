using BoerisCreaciones.Core.Models.Compras;

namespace BoerisCreaciones.Repository.Interfaces
{
    public interface IComprasRepository
    {
        public List<CompraVM> GetPurchases(List<char> filters, BusquedaCompra? search, string? orderCriteria, bool ascendingSort);
        public CompraVM GetPurchaseById(int idPurchase);
        public List<CompraVM> GetPurchasesByPartner(int idPartner);
        public List<MateriaPrimaCompraVM> GetPurchasedRawMaterials(int idPurchase);
        public CompraVM AddPurchase(NuevaCompra newPurchase);
        public void ReceivePurchase(int idPurchase, int idUser, RecepcionCompra purchaseReception);
        public void CancelPurchase(int idPurchase);
        public void DisablePurchase(int idPurchase);
        public void DeletePurchase(int idPurchase);
    }
}
