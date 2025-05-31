using System;
using System.Collections.Generic;

namespace PhoneStore.BusinessObjects.Models;

public partial class Voucher
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public decimal DiscountValue { get; set; }

    public string? DiscountType { get; set; }

    public decimal? MinOrderValue { get; set; }

    public DateTime ExpiryDate { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
