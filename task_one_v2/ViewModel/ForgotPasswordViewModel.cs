using System.ComponentModel.DataAnnotations;

namespace task_one_v2.ViewModel
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

}
