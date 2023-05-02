using MyPhotoDreamApp.Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace MyPhotoDreamApp.Domain.Entity
{

    public class User
    {
        public int Id { get; set; }

		[DataType(DataType.PhoneNumber)]
		public string PhoneNumber { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        public Role Role { get; set; }

        public Basket Basket { get; set; }
    }
}
