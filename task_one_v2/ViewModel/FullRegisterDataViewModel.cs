using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using task_one_v2.Models;

namespace task_one_v2.ViewModel
{
    public class FullRegisterDataViewModel
    {
        public decimal Userid { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;


        public string? Image { get; set; }

        public string? Nationality { get; set; }


        public string? CertificateImage { get; set; }

        public decimal Roleid { get; set; }
        public virtual Role Role { get; set; } = null!;


        public decimal VerificationStatusId { get; set; }
        public virtual UserVerificationStatus VerificationStatus { get; set; } = null!;


        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [DisplayName("Email")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; } = null!;
    }

}
