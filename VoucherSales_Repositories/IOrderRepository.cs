using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoucherSales_BO;

namespace VoucherSales_Repositories
{
    public interface IOrderRepository
    {
        void CreateOrder(Order order, List<OrderItem> items);
        List<Order> GetByUser(int userId);
        Order GetByID(int orderId);
        void UpdateOrderStatus(int orderId, string newStatus);
    }
}
