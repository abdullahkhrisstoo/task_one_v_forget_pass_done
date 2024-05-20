using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace task_one_v2.Models;

public partial class ContactU
{
    public decimal Contactid { get; set; }

    [Display(Name = "Full Name")]
    [Required(ErrorMessage = "Name is required")]
    [StringLength(20, ErrorMessage = "Name must be between 1 and 20 characters", MinimumLength = 1)]
    public string Name { get; set; } = null!;

    [Display(Name = "Email")]
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = null!;

    [Display(Name = "Subject")]
    [Required(ErrorMessage = "Subject is required")]
    [StringLength(20, ErrorMessage = "Subject must be between 1 and 20 characters", MinimumLength = 1)]
    public string Subject { get; set; } = null!;

    [Display(Name = "Message")]
    [Required(ErrorMessage = "Message is required")]
    [StringLength(500, ErrorMessage = "Message must be between 1 and 500 characters", MinimumLength = 1)]
    public string Message { get; set; } = null!;
}
