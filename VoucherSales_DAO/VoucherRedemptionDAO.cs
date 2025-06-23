using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VoucherSales_BO;

namespace VoucherSales_DAO
{
    public class VoucherRedemptionDAO
    {
        private readonly VoucherSalesDbContext _ctx = new();
        private static VoucherRedemptionDAO? _instance;
        public static VoucherRedemptionDAO Instance => _instance ??= new VoucherRedemptionDAO();

        // Lấy lịch sử Redeem của user
        public List<VoucherRedemption> GetByUser(int userId)
            => _ctx.VoucherRedemptions
                   .Include(r => r.Voucher)
                     .ThenInclude(v => v.VoucherType)
                   .Where(r => r.RedeemedByUserId == userId)
                   .ToList();
    }
}
