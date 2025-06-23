using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VoucherSales_BO;

namespace VoucherSales_DAO
{
    public class VoucherDAO
    {
        private static VoucherDAO _instance;
        private readonly VoucherSalesDbContext _ctx;
        public VoucherDAO()
        {
            _ctx = new VoucherSalesDbContext();
        }

        public static VoucherDAO Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new VoucherDAO();
                }
                return _instance;
            }

        }
        public List<Voucher> GetUnredeemedByUser(int userId)
        {
            return _ctx.Vouchers
                .Include(v => v.VoucherType)
                .Where(v => v.IssuedToUserId == userId && !v.IsRedeemed)
                .ToList();
        }

        public void MarkRedeemed(Guid voucherId, string location = null)
        {
            var v = _ctx.Vouchers.Find(voucherId);
            if (v == null) throw new Exception("Voucher not found");
            v.IsRedeemed = true;
            v.RedeemedAt = DateTime.Now;
            _ctx.SaveChanges();
            //  Lưu lịch sử
            _ctx.VoucherRedemptions.Add(new VoucherRedemption
            {
                VoucherId = voucherId,
                RedeemedByUserId = v.IssuedToUserId.Value,
                Location = location
            });
            _ctx.SaveChanges();
        }
    }
}
