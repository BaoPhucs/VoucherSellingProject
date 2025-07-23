using System;
using System.Collections.Generic;
using VoucherSales_BO;

namespace VoucherSales_Repositories
{
    public interface IOrderRepository
    {
        void CreateOrder(Order order, List<OrderItem> items);
        List<Order> GetByUser(int userId);
        Order GetByID(int orderId);
        void UpdateOrderStatus(int orderId, string newStatus);
        void DeleteOrder(int orderId);
        void UpdateOrder(Order order, List<OrderItem> items);

        void DeleteOrderItem(int orderItemId);
        void CreateOrderItem(OrderItem orderItem);
        void UpdateOrderItem(OrderItem orderItem);
        List<OrderItem> GetOrderItemsByOrderId(int orderId);
        List<Order> GetOrdersByDateRange(DateTime from, DateTime to);
    }
}