using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VoucherSales_BO;

namespace VoucherSales_DAO
{
    public class CartItemDAO
    {
        private readonly VoucherSalesDbContext _context;
        private static CartItemDAO? _instance;

        public CartItemDAO()
        {
            _context = new VoucherSalesDbContext();
        }
        public static CartItemDAO Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CartItemDAO();
                }
                return _instance;
            }
        }

        public void AddOrUpdate(int userId, int voucherTypeId, int qty = 1)
        {
            var item = _context.CartItems
                .FirstOrDefault(ci => ci.UserId == userId && ci.VoucherTypeId == voucherTypeId);
            if (item != null)
            {
                item.Quantity += qty;
                _context.Update(item);
            }
            else
            {
                _context.CartItems.Add(new CartItem
                {
                    UserId = userId,
                    VoucherTypeId = voucherTypeId,
                    Quantity = qty
                });
            }
            _context.SaveChanges();
        }

        public void UpdateQuantity(int cartItemId, int newQty)
        {
            var item = _context.CartItems.Find(cartItemId);
            if (item == null) return;
            item.Quantity = newQty;
            _context.SaveChanges();
        }

        public List<CartItem> GetByUser(int userId)
        {
            return _context.CartItems
               .Include(ci => ci.VoucherType)      // <-- nạp luôn navigation
               .Where(ci => ci.UserId == userId)
               .ToList();
        }

        public void Remove(int cartItemId)
        {
            var ci = _context.CartItems.Find(cartItemId);
            if (ci != null)
            {
                _context.CartItems.Remove(ci);
                _context.SaveChanges();
            }
        }

        public void Clear(int userId)
        {
            var list = _context.CartItems.Where(ci => ci.UserId == userId);
            _context.CartItems.RemoveRange(list);
            _context.SaveChanges();
        }
    }
}
