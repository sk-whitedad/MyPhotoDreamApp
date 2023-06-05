using MyPhotoDreamApp.Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace MyPhotoDreamApp.Domain.Entity
{

    public class User
    {
        public int Id { get; set; }

        [Display(Name = "Номер телефона")]
        [DataType(DataType.PhoneNumber)]
		public string PhoneNumber { get; set; }

        [Display(Name = "Пароль в зашифрованном виде")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Роль")]
        public Role Role { get; set; }

        public Basket Basket { get; set; }

        public List<ConfirmOrder>? ConfirmOrders { get; set; }
    }
}
