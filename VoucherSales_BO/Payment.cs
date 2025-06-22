using System;
using System.Collections.Generic;

namespace VoucherSales_BO;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int OrderId { get; set; }

    public string PaymentMethod { get; set; } 

    public decimal Amount { get; set; }

    public string Status { get; set; }

    public DateTime TransactionDate { get; set; }

    public string? TransactionRef { get; set; }

    public Order Order { get; set; } 
}
