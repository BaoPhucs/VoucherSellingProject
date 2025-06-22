using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoucherSales_BO;
using VoucherSales_DAO;

namespace VoucherSales_Repositories
{
    public class OrderRepository : IOrderRepository
    {
        public void CreateOrder(Order order, List<OrderItem> items) => 
            OrderDAO.Instance.CreateOrder(order, items);

        public Order GetByID(int orderId)
        {
            return OrderDAO.Instance.GetOrderById(orderId);
        }

        public List<Order> GetByUser(int userId) =>
            OrderDAO.Instance.GetOrdersByUser(userId);

        public void UpdateOrderStatus(int orderId, string newStatus)
        {
            OrderDAO.Instance.UpdateOrderStatus(orderId, newStatus);
        }
    }
}
