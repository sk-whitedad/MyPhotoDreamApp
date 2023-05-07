using MyPhotoDreamApp.Domain.Entity;
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

        public string Name { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public string DateCreated { get; set; }

        public string CategoryName { get; set; }

        public string ProductName { get; set; }

    }
}
