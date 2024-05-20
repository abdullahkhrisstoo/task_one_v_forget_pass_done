using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace task_one_v2.ViewModel
{
    public partial class UserLogin
    {

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
