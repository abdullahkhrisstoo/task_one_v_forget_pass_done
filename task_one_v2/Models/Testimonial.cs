using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace task_one_v2.Models;

public partial class Testimonial
{
    public decimal TestimonialId { get; set; }
    public decimal? UserId { get; set; }
    [DisplayName("Testimonial Text Area")]
    [MaxLength(255),MinLength(40)]
    public string TestimonialText { get; set; } = null!;
    public DateTime? DateTime { get; set; }
    [DisplayName("Approval Status")]
    public decimal? ApprovalStatusId { get; set; }
    public string? Userimg { get; set; }
    public virtual TestimonialApprovalStatus? ApprovalStatus { get; set; } /* = null!;*/
    public virtual UserInfo? User { get; set; } /*= null!;*/
}
