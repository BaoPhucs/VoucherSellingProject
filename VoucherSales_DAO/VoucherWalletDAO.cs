using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VoucherSales_BO;

namespace VoucherSales_DAO
{
    public class VoucherWalletDAO
    {
        private readonly VoucherSalesDbContext _ctx = new();
        private static VoucherWalletDAO? _instance;
        public static VoucherWalletDAO Instance => _instance ??= new VoucherWalletDAO();

        public List<VoucherType> GetWalletForUser(int userId)
        {
            // 1) Issued per type
            var issued = _ctx.OrderItems
                .Where(oi => oi.Order.UserId == userId
                          && oi.Order.PaymentStatus == "Success")
                .GroupBy(oi => oi.VoucherTypeId)
                .ToDictionary(g => g.Key, g => g.Sum(oi => oi.Quantity));

            // 2) Redeemed per type
            var redeemed = _ctx.VoucherRedemptions
                .Where(r => r.RedeemedByUserId == userId)
                .GroupBy(r => r.Voucher.VoucherTypeId)
                .ToDictionary(g => g.Key, g => g.Count());

            // 3) Build result
            var types = _ctx.VoucherTypes.ToList();
            var result = new List<VoucherType>();
            foreach (var vt in types)
            {
                vt.TotalIssued = issued.TryGetValue(vt.VoucherTypeId, out var i) ? i : 0;
                vt.RedeemedCount = redeemed.TryGetValue(vt.VoucherTypeId, out var r) ? r : 0;
                // AvailableCount là NotMapped => tự động tính

                if (vt.AvailableCount > 0)
                {
                    result.Add(vt);
                }
            }



            return result;

        }
    }
}
