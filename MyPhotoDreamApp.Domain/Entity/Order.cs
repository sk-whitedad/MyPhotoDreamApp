using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPhotoDreamApp.Domain.Entity
{
	public class Order
	{
		public int Id { get; set; }

		public DateTime DateCreated { get; set; }

        public int Quantity { get; set; }

        public string FirstName { get; set; }

		public string LastName { get; set; }

        public string Address { get; set; }

		public int? ProductId { get; set; }

		public int? BasketId { get; set; }

		public virtual Basket Basket { get; set; }


	}
}
