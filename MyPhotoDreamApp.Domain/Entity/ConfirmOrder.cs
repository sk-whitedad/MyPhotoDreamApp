using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPhotoDreamApp.Domain.Entity
{
    public class ConfirmOrder
    {
        public string Id { get; set; }
        public string PhoneNumber { get; set; }
        public decimal SummOrder { get; set; }
        //public List<Order> OrderList { get; set; }
    }
}
