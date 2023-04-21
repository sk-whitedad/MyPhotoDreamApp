using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MyPhotoDreamApp.ViewModels
{
    public class UserViewModel
    {
        [Remote(action: "CheckPhoneNumber", controller: "Home", ErrorMessage = "Этот номер уже используется")]
        [Required(ErrorMessage = "Укажите номер телефона")]
        [DataType(DataType.PhoneNumber)]
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
