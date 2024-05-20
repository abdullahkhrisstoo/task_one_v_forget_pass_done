using System;
using System.Collections.Generic;

namespace task_one_v2.Models;

public partial class Aboutu
{
    public decimal Aboutid { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Yearsofexperience { get; set; }

    public decimal Numberofchef { get; set; }

    public string Image1 { get; set; } = null!;

    public string Image2 { get; set; } = null!;

    public string? Image3 { get; set; }

    public string? Image4 { get; set; }
}
