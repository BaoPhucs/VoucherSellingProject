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

        /// <summary>
        /// Sinh N voucher cho mỗi OrderItem đã thanh toán thành công
        /// </summary>
        public void GenerateForOrder(int orderId)
        {
            // 1) Lấy Order và các OrderItem
            var items = _ctx.OrderItems
                .Include(oi => oi.Order)
                .Where(oi => oi.OrderId == orderId && oi.Order.PaymentStatus == "Success")
                .ToList();

            // 2) Với mỗi OrderItem, tạo đúng Quantity bản ghi Voucher
            foreach (var oi in items)
            {
                var userId = oi.Order.UserId;
                for (int i = 0; i < oi.Quantity; i++)
                {
                    // generate code, ví dụ: TYPEID-YYYYMMDD-N        
                    var code = $"{oi.VoucherTypeId}-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8)}";

                    _ctx.Vouchers.Add(new Voucher
                    {
                        VoucherTypeId = oi.VoucherTypeId,
                        Code = code,
                        IsRedeemed = false,
                        IssuedToUserId = userId,
                        CreatedAt = DateTime.Now
                    });
                }
            }

            _ctx.SaveChanges();
        }
    }
}
