using System;
using System.Collections.Generic;

namespace VoucherSales_BO;

public partial class Voucher
{
    public Guid VoucherId { get; set; }

    public int VoucherTypeId { get; set; }

    public string Code { get; set; } = null!;

    public bool IsRedeemed { get; set; }

    public DateTime? RedeemedAt { get; set; }

    public int? OrderId { get; set; }

    public int? IssuedToUserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public User? IssuedToUser { get; set; }

    public Order? Order { get; set; }

    public ICollection<VoucherRedemption> VoucherRedemptions { get; set; } 

    public VoucherType VoucherType { get; set; }
}
