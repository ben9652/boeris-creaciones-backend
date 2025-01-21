using BoerisCreaciones.Core.Models.Compras;

namespace BoerisCreaciones.Service.Interfaces
{
    public interface IComprasService
    {
        public List<CompraDTO> GetPurchases(int userId);
        public CompraDTO GetPurchaseById(int idPurchase, int userId);
        public List<CompraDTO> GetPurchasesByPartner(int idPartner, int userId);
        public List<MateriaPrimaCompraDTO> GetPurchasedRawMaterials(int idPurchase, int userId);
        public CompraDTO AddPurchase(NuevaCompraDTO newPurchase);
        public void ReceivePurchase(int idPurchase, int userId, int idBranch);
        public void CancelPurchase(int idPurchase);
        public void DisablePurchase(int idPurchase);
        public void DeletePurchase(int idPurchase);
    }
}
