using System;
using System.Collections.Generic;

namespace task_one_v2.Models;

public partial class Payment
{
    public decimal Paymentid { get; set; }

    public decimal Userid { get; set; }

    public string Fullname { get; set; } = null!;

    public string Numberid { get; set; } = null!;

    public string Cvc { get; set; } = null!;

    public DateTime Expiredate { get; set; }

    public decimal Amount { get; set; }

    public virtual UserInfo? User { get; set; } /*= null!;*/
}
