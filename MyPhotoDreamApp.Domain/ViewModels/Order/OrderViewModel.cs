using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPhotoDreamApp.Domain.ViewModels.Order
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        public string ProductName { get; set; }

        public string CategoryName { get; set; }

        public decimal Price { get; set; }

		public int Quantity { get; set; }

		public string Address { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DateCreate { get; set; }
    }
}
