using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace task_one_v2.Models;

public partial class UserInfo
{
    public decimal Userid { get; set; }


    [Required(ErrorMessage = "First name is required")]
    [DisplayName("First Name")]
    [MaxLength(10), MinLength(3)]

    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Last name is required")]
    [DisplayName("Last Name")]
    [MaxLength(10),MinLength(3)]
    public string LastName { get; set; } = null!;

    [NotMapped]
    [DisplayName("Personal Image")]
    public IFormFile? PersonalImageFile { get; set; }
    public string? Image { get; set; }

    [DisplayName("NATIONALITY")]
    [MaxLength(15), MinLength(3)]

    public string? Nationality { get; set; }

    public decimal Roleid { get; set; }

    public decimal VerificationStatusId { get; set; }

    public string? CertificateImage { get; set; }

    public virtual ICollection<Chatroom> ChatroomChefs { get; set; } = new List<Chatroom>();

    public virtual ICollection<Chatroom> ChatroomUsers { get; set; } = new List<Chatroom>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<RecipeOrder> RecipeOrders { get; set; } = new List<RecipeOrder>();

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();

    public virtual Role? Role { get; set; }/* = null!;*/
    
    public virtual ICollection<Testimonial> Testimonials { get; set; } = new List<Testimonial>();

    public virtual ICollection<UserCredential> UserCredentials { get; set; } = new List<UserCredential>();

    public virtual UserVerificationStatus? VerificationStatus { get; set; } /*= null!;*/
}
