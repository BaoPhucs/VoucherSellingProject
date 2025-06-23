using System;
using System.Collections.Generic;

namespace VoucherSales_BO;

public partial class Order
{
    public int OrderId { get; set; }

    public int UserId { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal TotalAmount { get; set; }

    public string PaymentStatus { get; set; } = null!;

    public string? Notes { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; }

    public User User { get; set; } 

    //public ICollection<VoucherRedemption> VoucherRedemptions { get; set; } 

    //public ICollection<Voucher> Vouchers { get; set; } 
}
