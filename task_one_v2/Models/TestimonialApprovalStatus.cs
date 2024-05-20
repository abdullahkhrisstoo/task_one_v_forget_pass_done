using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace task_one_v2.Models;

public partial class TestimonialApprovalStatus
{
    [DisplayName("Approval Status")]
    public decimal ApprovalStatusId { get; set; }

    public string StatusName { get; set; } = null!;

    public virtual ICollection<Testimonial> Testimonials { get; set; } = new List<Testimonial>();
}
