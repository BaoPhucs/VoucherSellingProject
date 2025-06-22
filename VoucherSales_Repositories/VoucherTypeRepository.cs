using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoucherSales_BO;
using VoucherSales_DAO;

namespace VoucherSales_Repositories
{
    public class VoucherTypeRepository : IVoucherTypeRepository
    {
        public List<VoucherType> GetAll()
        {
            return VoucherTypeDAO.Instance.GetAllVoucherTypes();
        }
    }
}
