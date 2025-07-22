using System;
using System.Collections.Generic;
using VoucherSales_BO;
using VoucherSales_DAO;

namespace VoucherSales_Repositories
{
    public class OrderRepository : IOrderRepository
    {
        public void CreateOrder(Order order, List<OrderItem> items) =>
            OrderDAO.Instance.CreateOrder(order, items);

        public Order GetByID(int orderId) =>
            OrderDAO.Instance.GetOrderById(orderId);

        public List<Order> GetByUser(int userId) =>
            OrderDAO.Instance.GetOrdersByUser(userId);

        public void UpdateOrderStatus(int orderId, string newStatus) =>
            OrderDAO.Instance.UpdateOrderStatus(orderId, newStatus);

        public void DeleteOrder(int orderId)
        {
            if (orderId <= 0)
                throw new ArgumentException("OrderId must be greater than zero.", nameof(orderId));

            OrderDAO.Instance.DeleteOrder(orderId);
        }

        public void UpdateOrder(Order order, List<OrderItem> items)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));
            if (order.OrderId <= 0)
                throw new ArgumentException("OrderId must be greater than zero.", nameof(order.OrderId));

            OrderDAO.Instance.UpdateOrder(order, items);
        }

        public void DeleteOrderItem(int orderItemId)
        {
            if (orderItemId <= 0)
                throw new ArgumentException("OrderItemId must be greater than zero.", nameof(orderItemId));

            OrderDAO.Instance.DeleteOrderItem(orderItemId);
        }

        public List<OrderItem> GetOrderItemsByOrderId(int orderId)
        {
            if (orderId <= 0)
                throw new ArgumentException("OrderId must be greater than zero.", nameof(orderId));

            return OrderDAO.Instance.GetOrderItemsByOrderId(orderId);
        }

        public void CreateOrderItem(OrderItem orderItem)
        {
            if (orderItem == null)
                throw new ArgumentNullException(nameof(orderItem));

            OrderDAO.Instance.CreateOrderItem(orderItem);
        }

        public void UpdateOrderItem(OrderItem orderItem)
        {
            if (orderItem == null)
                throw new ArgumentNullException(nameof(orderItem));

            OrderDAO.Instance.UpdateOrderItem(orderItem);
        }

        public List<Order> GetOrdersByDateRange(DateTime from, DateTime to)
        {
            return OrderDAO.Instance.GetOrdersByDateRange(from, to);
        }
    }
}
