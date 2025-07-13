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
