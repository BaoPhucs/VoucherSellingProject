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

        public List<Voucher> GetMyWalletVouchers(int userId)
        {
            return VoucherDAO.Instance.GetUnredeemedByUser(userId);
        }

        public void Redeem(Guid voucherId, string location = null) => VoucherDAO.Instance.MarkRedeemed(voucherId, location);

        public void AddVoucher(Voucher voucher)
        {
            if (voucher == null) throw new ArgumentNullException(nameof(voucher));
            VoucherDAO.Instance.AddVoucher(voucher);
        }
        public void DeleteVoucher(Guid voucherId)
        {
            if (voucherId == Guid.Empty) throw new ArgumentException("Voucher ID cannot be empty", nameof(voucherId));
            VoucherDAO.Instance.DeleteVoucher(voucherId);
        }
    }
}
         