using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoucherSales_BO;

namespace VoucherSales_DAO
{
    public class VoucherTypeDAO
    {
        private static VoucherTypeDAO? _instance;
        private readonly VoucherSalesDbContext _context;

        private VoucherTypeDAO()
        {
            _context = new VoucherSalesDbContext();
        }

        public static VoucherTypeDAO Instance
            => _instance ??= new VoucherTypeDAO();

        public List<VoucherType> GetAllVoucherTypes()
        {
            return _context.VoucherTypes.ToList();
        }
    }
}
