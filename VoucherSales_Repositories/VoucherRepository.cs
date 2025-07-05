using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoucherSales_BO;
using VoucherSales_DAO;

namespace VoucherSales_Repositories
{
    public class VoucherRepository : IVoucherRepository
    {
        public void GenerateForOrder(int orderId) => VoucherDAO.Instance.GenerateForOrder(orderId);

        public List<Voucher> GetMyWallet(int userId)
        {
            return VoucherDAO.Instance.GetUnredeemedByUser(userId);
        }

        public void Redeem(Guid voucherId, string location = null) => VoucherDAO.Instance.MarkRedeemed(voucherId, location);
     
    }
}
         