using System;
using System.Collections.Generic;

namespace VoucherSales_BO;

public partial class VoucherRedemption
{
    public int RedemptionId { get; set; }

    public Guid VoucherId { get; set; }

    public int RedeemedByUserId { get; set; }

    public DateTime RedeemedAt { get; set; }

    public int? OrderId { get; set; }

    public string? Location { get; set; }

    public Order? Order { get; set; }

    public User RedeemedByUser { get; set; } 

    public Voucher Voucher { get; set; } 
}
