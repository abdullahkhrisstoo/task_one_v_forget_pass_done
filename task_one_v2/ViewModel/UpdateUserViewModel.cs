using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace task_one_v2.ViewModel
{
    public class UpdateUserViewModel
    {
        public string? ImagePath { get; set; } = null;

        public string? CertificateImagePath { get; set; } = null;

        [Required(ErrorMessage = "Personal Image is required")]
        [DisplayName("Personal Image")]
        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        [Required(ErrorMessage = "Certificate Image is required")]
        [DisplayName("Certificate Image")]
        [NotMapped]
        public IFormFile? CertificateImageFile { get; set; }
        public string? Nationality { get; set; }
    }
}
