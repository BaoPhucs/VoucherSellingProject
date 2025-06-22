using System;
using System.Collections.Generic;

namespace VoucherSales_BO;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public int RoleId { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsActive { get; set; }

    public ICollection<Order> Orders { get; set; } 

    public Role Role { get; set; } 

    public ICollection<VoucherRedemption> VoucherRedemptions { get; set; } 

    public ICollection<Voucher> Vouchers { get; set; }

    public ICollection<CartItem> CartItems { get; set; }
}
