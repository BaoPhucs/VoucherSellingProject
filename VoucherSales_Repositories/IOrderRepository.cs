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

        bool DeleteOrder(int orderId);
        void UpdateOrder(Order order, List<OrderItem> items);

        //write update OrderItem and DeleteOrderItem
        //based on the orderId since an item belongs to an order 
        void DeleteOrderItem(int orderItemId);

        void CreateOrderItem(OrderItem orderItem);
        void UpdateOrderItem(OrderItem orderItem);
        List<OrderItem> GetOrderItemsByOrderId(int orderId);

        void UpdateOrderTotal(int orderId);









    }
}
