using System;
using System.Collections.Generic;

namespace VoucherSales_BO;

public partial class OrderItem
{
    public int OrderItemId { get; set; }

    public int OrderId { get; set; }

    public int VoucherTypeId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal Subtotal { get; set; }

    public  Order Order { get; set; } 

    public VoucherType VoucherType { get; set; } 
}
