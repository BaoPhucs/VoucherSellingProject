using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoucherSales_BO;
using VoucherSales_DAO;

namespace VoucherSales_Repositories
{
    public class CartItemRepository : ICartItemRepository
    {
        public void AddOrUpdate(int userId, int voucherTypeId, int qty = 1) =>
            CartItemDAO.Instance.AddOrUpdate(userId, voucherTypeId, qty);


        public void Clear(int userId) =>
            CartItemDAO.Instance.Clear(userId);

        public List<CartItem> GetByUser(int userId) =>
            CartItemDAO.Instance.GetByUser(userId);

        public void Remove(int cartItemId) =>
            CartItemDAO.Instance.Remove(cartItemId);

        public void UpdateQuantity(int cartItemId, int newQty) =>
            CartItemDAO.Instance.UpdateQuantity(cartItemId, newQty);
    }
}
