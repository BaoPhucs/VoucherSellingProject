using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoucherSales_BO;
using VoucherSales_DAO;

namespace VoucherSales_Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        public void CreatePayment(Payment payment)
        {
            PaymentDAO.Instance.CreatePayment(payment);
        }
            
        public Payment? GetByOrder(int orderId)
        {
            return PaymentDAO.Instance.GetByOrder(orderId);
        }
    }
}
