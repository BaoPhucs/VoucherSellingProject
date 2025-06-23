using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoucherSales_BO;

public partial class Voucher
{
    public Guid VoucherId { get; set; }

    public int VoucherTypeId { get; set; }

    public string Code { get; set; } = null!;

    public bool IsRedeemed { get; set; }

    public DateTime? RedeemedAt { get; set; }

    public int? IssuedToUserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public User? IssuedToUser { get; set; }

    public ICollection<VoucherRedemption> VoucherRedemptions { get; set; } 

    public VoucherType VoucherType { get; set; }


    [NotMapped]
    public bool IsSelected { get; set; }
}
