using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace task_one_v2.ViewModel
{
    public class EditPasswordViewModel
    {
        public decimal? UserCredentialId { get; set; }
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [DisplayName("Last password")]
        public string LastPassword { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [DisplayName("New password")]
        public string NewPassword { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [DisplayName("Confirm password")]
        public string ConfirmPassword { get; set; } = null!;

        public decimal? UserInfoId { get; set; }
    }
}

