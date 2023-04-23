using MyPhotoDreamApp.Domain.Enum;
using System.Data;

namespace MyPhotoDreamApp.Domain.Entity
{

    public class User
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
    }
}
