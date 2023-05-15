using System.ComponentModel.DataAnnotations;

namespace MyPhotoDreamApp.Domain.ViewModels.Account
{
    public class RegisterViewModel
    {
		[DataType(DataType.PhoneNumber)]
		[MaxLength(10, ErrorMessage = "Номер телефона должен иметь длину 10 символов")]
        [MinLength(10, ErrorMessage = "Номер телефона должен иметь длину 10 символов")]
        [Required(ErrorMessage = "Укажите номер телефона")]
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Укажите пароль")]
        [MinLength(6, ErrorMessage = "Пароль должен иметь длину больше 6 символов")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Подтвердите пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [Display(Name = "Пароль")]
        public string PasswordConfirm { get; set; }
    }
}