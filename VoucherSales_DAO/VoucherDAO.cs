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
        //get all vouchers

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
            using var tx = _ctx.Database.BeginTransaction();

            // 1) Lấy Order kèm OrderItems
            var order = _ctx.Orders
                .Include(o => o.OrderItems)
                .First(o => o.OrderId == orderId);

            // 2) Gom nhóm theo loại voucher, tính tổng Quantity
            var grouped = order.OrderItems
                .GroupBy(oi => oi.VoucherTypeId)
                .Select(g => new
                {
                    VoucherTypeId = g.Key,
                    TotalQty = g.Sum(oi => oi.Quantity)
                });

            foreach (var grp in grouped)
            {
                // 3) Lấy kiểu voucher
                var vt = _ctx.VoucherTypes.Find(grp.VoucherTypeId);
                if (vt == null)
                    throw new Exception($"Không tìm thấy VoucherType {grp.VoucherTypeId}.");

                // 4) Kiểm tra còn đủ
                if (vt.TotalQuantity < grp.TotalQty)
                    throw new Exception(
                        $"Loại '{vt.Name}' chỉ còn {vt.TotalQuantity} mã, bạn yêu cầu {grp.TotalQty}.");

                // 5) Giảm TotalQuantity 1 lần
                vt.TotalQuantity -= grp.TotalQty;
                _ctx.VoucherTypes.Update(vt);

                // 6) Phát hành đúng số TotalQty
                for (int i = 0; i < grp.TotalQty; i++)
                {
                    _ctx.Vouchers.Add(new Voucher
                    {
                        VoucherTypeId = vt.VoucherTypeId,
                        IssuedToUserId = order.UserId,
                        Code = $"{vt.VoucherTypeId}-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid():N}".Substring(0, 24),
                        IsRedeemed = false,
                        CreatedAt = DateTime.Now
                    });
                }
            }

            _ctx.SaveChanges();
            tx.Commit();
        }

        //AddVoucher
        public void AddVoucher(Voucher voucher)
        {
            if (voucher == null) throw new ArgumentNullException(nameof(voucher));
            _ctx.Vouchers.Add(voucher);
            _ctx.SaveChanges();
        }
        //DeleteVoucher
        public void DeleteVoucher(Guid voucherId)
        {
            var voucher = _ctx.Vouchers.Find(voucherId);
            if (voucher == null) throw new Exception("Voucher not found");
            _ctx.Vouchers.Remove(voucher);
            _ctx.SaveChanges();
        }

    }
}
