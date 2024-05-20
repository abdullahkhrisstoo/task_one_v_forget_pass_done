using System;
using System.Collections.Generic;

namespace task_one_v2.Models;

public partial class RecipeOrder
{
    public decimal OrderId { get; set; }

    public decimal UserId { get; set; }

    public decimal RecipeId { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal? TotalPrice { get; set; }

    public virtual Recipe? Recipe { get; set; } /*= null!;*/

    public virtual UserInfo? User { get; set; } /*= null!;*/
}
