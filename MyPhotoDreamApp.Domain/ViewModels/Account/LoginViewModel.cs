using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MyPhotoDreamApp.Domain.ViewModels.Account
{
    public class LoginViewModel
    {
        [DataType(DataType.PhoneNumber)]
        [MaxLength(10, ErrorMessage = "Номер телефона должен иметь длину 10 символов")]
        [MinLength(10, ErrorMessage = "Номер телефона должен иметь длину 10 символов")]
        [Required(ErrorMessage = "Укажите номер телефона")]
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Укажите пароль")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

		public string? ReturnUrl { get; set; }//адрес с которого пришли на эту форму
	}
}
