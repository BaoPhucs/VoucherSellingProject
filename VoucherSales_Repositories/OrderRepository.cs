using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoucherSales_BO;
using VoucherSales_DAO;
using VoucherSales_Repositories;
namespace VoucherSales_Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private VoucherSalesDbContext _context;
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

        public bool DeleteOrder(int orderId)
        {
            //delete an order
            if (orderId <= 0) throw new ArgumentException("OrderId must be greater than zero.", nameof(orderId));
            return OrderDAO.Instance.DeleteOrder(orderId);


        }

        void IOrderRepository.UpdateOrder(Order order, List<OrderItem> items)
        {
            //update an order
            if (order == null) throw new ArgumentNullException(nameof(order));
            if (order.OrderId <= 0) throw new ArgumentException("OrderId must be greater than zero.", nameof(order.OrderId));
            OrderDAO.Instance.UpdateOrder(order, items);
        }
        // item crud



        void IOrderRepository.DeleteOrderItem(int orderItemId)
        {
            //delete an item in an order
            if (orderItemId <= 0) throw new ArgumentException("OrderItemId must be greater than zero.", nameof(orderItemId));
            OrderDAO.Instance.DeleteOrderItem(orderItemId);
        }

        List<OrderItem> IOrderRepository.GetOrderItemsByOrderId(int orderId)
        {

            if (orderId <= 0) throw new ArgumentException("OrderId must be greater than zero.", nameof(orderId));
            return OrderDAO.Instance.GetOrderItemsByOrderId(orderId);
        }

        void IOrderRepository.CreateOrderItem(OrderItem orderItem)
        {

            if (orderItem == null) throw new ArgumentNullException(nameof(orderItem));
            if (orderItem.OrderItemId <= 0) throw new ArgumentException("OrderItemId must be greater than zero.", nameof(orderItem.OrderItemId));
            OrderDAO.Instance.CreateOrderItem(orderItem);
        }

        void IOrderRepository.UpdateOrderItem(OrderItem orderItem)
        {
            if (orderItem == null) throw new ArgumentNullException(nameof(orderItem));
            if (orderItem.OrderItemId <= 0) throw new ArgumentException("OrderItemId must be greater than zero.", nameof(orderItem.OrderItemId));
            OrderDAO.Instance.UpdateOrderItem(orderItem);
        }

        //implement in repo
        void IOrderRepository.UpdateOrderTotal(int orderId) =>
            OrderDAO.Instance.UpdateOrderTotal(orderId);
    }
}
