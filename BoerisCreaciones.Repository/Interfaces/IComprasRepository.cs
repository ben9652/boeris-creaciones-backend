using BoerisCreaciones.Core.Models.Compras;

namespace BoerisCreaciones.Repository.Interfaces
{
    public interface IComprasRepository
    {
        public List<CompraVM> GetPurchases();
        public CompraVM GetPurchaseById(int idPurchase);
        public List<CompraVM> GetPurchasesByPartner(int idPartner);
        public List<MateriaPrimaCompraVM> GetPurchasedRawMaterials(int idPurchase);
        public CompraVM AddPurchase(NuevaCompraDTO newPurchase);
        public void ReceivePurchase(int idPurchase, int idUser, int idBranch);
        public void CancelPurchase(int idPurchase);
        public void DisablePurchase(int idPurchase);
        public void DeletePurchase(int idPurchase);
    }
}
