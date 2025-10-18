using System.ComponentModel.DataAnnotations;

namespace Ecommerce.ViewModels
{
    public class LoginVM
    {
        [Display(Name = "UserName")]
        [Required(ErrorMessage = "chua nhap")]
        [MaxLength(20, ErrorMessage = "Maximum 20 characters allowed")]
        public string UserName { get; set; }


        [Display(Name = "Password")]
        [Required(ErrorMessage = "chua nhap")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
