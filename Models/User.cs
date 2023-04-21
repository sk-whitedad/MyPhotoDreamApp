using Microsoft.EntityFrameworkCore;
using MyPhotoDreamApp.Enum;

namespace MyPhotoDreamApp.Models
{
    
    public class User
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
    }
}
