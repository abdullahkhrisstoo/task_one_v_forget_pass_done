using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace task_one_v2.ViewModel
{
    public class UpdateUserDataViewModel
    {
        //UserInfo Id
        public decimal? UserInfoId { get; set; }


        [Required(ErrorMessage = "First name is required")]
        [DisplayName("First Name")]
        [MaxLength(10), MinLength(3)]

        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last name is required")]
        [DisplayName("Last Name")]
        [MaxLength(10), MinLength(3)]
        public string LastName { get; set; } = null!;

        [DisplayName("Personal Image")]
        public IFormFile? PersonalImageFile { get; set; }
        [DisplayName("Personal Image")]
        public string? Image { get; set; }

        [DisplayName("NATIONALITY")]
        [MaxLength(15), MinLength(3)]

        public string? Nationality { get; set; }

        public decimal? Roleid { get; set; }

        public decimal? VerificationStatusId { get; set; }

        [DisplayName("Certificate Image")]
        public IFormFile? CertificateImageFile { get; set; }
        [DisplayName("Certificate Image")]
        public string? CertificateImage { get; set; }

        ///UserCredential
        public decimal? UserCredentialId { get; set; }

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
