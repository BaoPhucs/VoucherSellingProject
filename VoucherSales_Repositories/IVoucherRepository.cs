using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoucherSales_BO;

namespace VoucherSales_Repositories
{
    public interface IVoucherRepository
    {
        List<Voucher> GetMyWallet(int userId);
        void Redeem(Guid voucherId, string location = null);
        void GenerateForOrder(int orderId);
    }
}
