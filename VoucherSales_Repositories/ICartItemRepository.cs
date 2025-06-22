using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoucherSales_BO;

namespace VoucherSales_Repositories
{
    public interface ICartItemRepository
    {
        void AddOrUpdate(int userId, int voucherTypeId, int qty = 1);
        List<CartItem> GetByUser(int userId);
        void UpdateQuantity(int cartItemId, int newQty);
        void Remove(int cartItemId);
        void Clear(int userId);
    }
}
