using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoucherSales_BO;

namespace VoucherSales_DAO
{
    public class PaymentDAO
    {
        private readonly VoucherSalesDbContext _ctx = new();
        private static PaymentDAO? _instance;
        public static PaymentDAO Instance => _instance ??= new PaymentDAO();

        public void CreatePayment(Payment p)
        {
            _ctx.Payments.Add(p);
            _ctx.SaveChanges();
        }

        public Payment? GetByOrder(int orderId)
        {
            return _ctx.Payments.FirstOrDefault(x => x.OrderId == orderId);
        }
    }
}
