using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoucherSales_BO;
using VoucherSales_DAO;

namespace VoucherSales_Repositories
{
    public class VoucherWalletRepository : IVoucherWalletRepository
    {
        public List<VoucherType> GetWalletForUser(int userId)
        {
            return VoucherWalletDAO.Instance.GetWalletForUser(userId);
        }
    }
}
