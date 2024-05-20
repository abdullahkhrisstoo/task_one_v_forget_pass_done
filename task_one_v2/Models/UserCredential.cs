using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace task_one_v2.Models;

public partial class UserCredential
{
    public decimal Id { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [DisplayName("Email")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [DisplayName("Password")]
    public string Password { get; set; } = null!;

    public decimal Userid { get; set; }

    public virtual UserInfo? User { get; set; } /*= null!;*/
}

