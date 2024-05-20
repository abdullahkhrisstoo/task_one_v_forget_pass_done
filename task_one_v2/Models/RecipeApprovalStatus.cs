using System;
using System.Collections.Generic;

namespace task_one_v2.Models;

public partial class RecipeApprovalStatus
{
    public decimal ApprovalStatusId { get; set; }

    public string StatusName { get; set; } = null!;

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}
