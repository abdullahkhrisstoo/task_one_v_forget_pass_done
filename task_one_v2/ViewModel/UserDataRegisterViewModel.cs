using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace task_one_v2.ViewModel
{
    public class UserDataRegisterViewModel
    {
        //[Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [DisplayName("Email")]
        public string? Email { get; set; } /*= null!;*/

        //[Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string? Password { get; set; }/* = null!;*/

        [Required(ErrorMessage = "First name is required")]
        [DisplayName("First Name")]
        public string? FirstName { get; set; }/* = null!;*/

        [Required(ErrorMessage = "Last name is required")]
        [DisplayName("Last Name")]
        public string? LastName { get; set; }/* = null!;*/

        [DisplayName("Is Chef")]
        public bool IsChef { get; set; }
        /////////////////////////////////
        [NotMapped]
        [DisplayName("Personal Image")]
        public IFormFile? PersonalImageFile { get; set; }
        public string? Image { get; set; }

        [DisplayName("NATIONALITY")]
        [MaxLength(15), MinLength(3)]

        public string? Nationality { get; set; }
    }
}
