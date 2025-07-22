using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VoucherSales_BO;

namespace VoucherSales_DAO
{
    public class OrderDAO
    {
        private static OrderDAO? _instance;
        private readonly VoucherSalesDbContext _context;
        private OrderDAO()
        {
            _context = new VoucherSalesDbContext();
        }
        public static OrderDAO Instance
            => _instance ??= new OrderDAO();

        public void CreateOrder(Order order, List<OrderItem> items)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();

            foreach (var it in items)
            {
                it.OrderId = order.OrderId;
                _context.OrderItems.Add(it);
            }

            _context.SaveChanges();
        }

        //DeleteOrder()
        public void DeleteOrder(int orderId)
        {
            var order = _context.Orders.Find(orderId);
            if (order != null)
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }
        }

        //UpdateOrder()
        public void UpdateOrder(Order order, List<OrderItem> items)
        {
            var existingOrder = _context.Orders.Find(order.OrderId);
            if (existingOrder != null)
            {
                existingOrder.PaymentStatus = order.PaymentStatus;
                existingOrder.UserId = order.UserId;
                _context.SaveChanges();
                // Update or add items
                foreach (var item in items)
                {
                    var existingItem = _context.OrderItems.Find(item.OrderItemId);
                    if (existingItem != null)
                    {
                        existingItem.Quantity = item.Quantity;
                        existingItem.VoucherTypeId = item.VoucherTypeId;
                    }
                    else
                    {
                        item.OrderId = order.OrderId; // Ensure OrderId is set for new items
                        _context.OrderItems.Add(item);
                    }
                }
                _context.SaveChanges();
            }
        }

        //DeleteOrderItem() to remove an item from an order
        public void DeleteOrderItem(int orderItemId)
        {
            var item = _context.OrderItems.Find(orderItemId);
            if (item != null)
            {
                _context.OrderItems.Remove(item);
                _context.SaveChanges();
            }
        }

        //create Order item
        public void CreateOrderItem(OrderItem item)
        {
            _context.OrderItems.Add(item);
            _context.SaveChanges();
        }

        //update an item in an order
        public void UpdateOrderItem(OrderItem item)
        {
            var existingItem = _context.OrderItems.Find(item.OrderItemId);
            if (existingItem != null)
            {
                existingItem.Quantity = item.Quantity;
                existingItem.VoucherTypeId = item.VoucherTypeId;
                _context.SaveChanges();
            }
        }
        // UI: click an order to see its items
        public List<OrderItem> GetOrderItemsByOrderId(int orderId)
        {
            return _context.OrderItems
                .Include(oi => oi.VoucherType)
                .Where(oi => oi.OrderId == orderId)
                .ToList();
        }
        /// <summary> Lấy tất cả orders của user, bao gồm items và voucherType </summary>
        public List<Order> GetOrdersByUser(int userId)
        {
            return _context.Orders
            .Include(o => o.OrderItems)
               .ThenInclude(oi => oi.VoucherType)
            .Where(o => o.UserId == userId)
            .ToList();
        }

        public Order? GetOrderById(int orderId)
        {
            return _context.Orders
            .Include(o => o.OrderItems)
               .ThenInclude(oi => oi.VoucherType)
            .FirstOrDefault(o => o.OrderId == orderId)!;
        }

        //check constraint : Failed Success or Pending
        public void UpdateOrderStatus(int orderId, string newStatus)
        {
            var o = _context.Orders.Find(orderId);
            if (o != null)
            {
                o.PaymentStatus = newStatus;
                _context.SaveChanges();
            }
        }
    }
}
