using System.ComponentModel.DataAnnotations;

namespace WebMicrowaveLine.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "NameRequired")]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "PasswordRequired")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember?")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}
