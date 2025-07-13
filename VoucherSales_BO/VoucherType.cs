using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoucherSales_BO;

public partial class VoucherType
{
    public int VoucherTypeId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string DiscountType { get; set; } = null!;

    public decimal DiscountValue { get; set; }

    public decimal? MinOrderValue { get; set; }

    public int TotalQuantity { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime ValidFrom { get; set; }

    public DateTime ValidTo { get; set; }

    public string Category { get; set; }

    public string Location { get; set; }

    public string? ImageUrl { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; }

    public ICollection<Voucher> Vouchers { get; set; }

    public ICollection<CartItem> CartItems { get; set; }

    [NotMapped]
    public int TotalIssued { get; set; }

    [NotMapped]
    public int RedeemedCount { get; set; }

    [NotMapped]
    public int AvailableCount => TotalIssued - RedeemedCount;
}
