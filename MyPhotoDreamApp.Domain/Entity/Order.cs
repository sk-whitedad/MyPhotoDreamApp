using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MyPhotoDreamApp.Domain.Entity
{
	public class Order
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string PhoneNumber { get; set; }
 
		public int Quantity { get; set; }

		public decimal Price { get; set; }

        public DateTime DateCreated { get; set; }

		public bool CheckConfirm { get; set; } = false;

        public int ProductId { get; set; }

		public int? BasketId { get; set; }

		public virtual Basket? Basket { get; set; }

		public int? ConfirmOrderId { get; set; }

		public ConfirmOrder? ConfirmOrder { get; set; }
    }
}
