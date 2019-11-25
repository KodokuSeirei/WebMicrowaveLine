using System.ComponentModel.DataAnnotations;

namespace WebMicrowaveLine.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "UserNameRequired")]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "FullNameRequired")]
        [Display(Name = "FullName")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "BirthdayRequired")]
        [DataType(DataType.Date)]
        [Display(Name = "Birthday")]
        public string Birthday { get; set; }

        [Required(ErrorMessage = "AdressRequired")]
        [Display(Name = "Adress")]
        public string Adress { get; set; }

        [Required(ErrorMessage = "EmailRequired")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "PasswordRequired")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "PasswordsDoNotMatch")]
        [DataType(DataType.Password)]
        [Display(Name = "PasswordConfirm")]
        public string PasswordConfirm { get; set; }
    }
}
