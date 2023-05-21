using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPhotoDreamApp.Domain.Entity
{
    public class ConfirmOrder
    {
        public int Id { get; set; }

        public decimal SummOrder { get; set; }

        public string? DeliveryAddress { get; set; }

        public bool CheckDelivery { get; set; }

        public virtual List<Order>? Orders { get; set; }

		public DateTime DateCreated { get; set; }

		public int UserId { get; set; }

        public User User { get; set; }
    }

}
