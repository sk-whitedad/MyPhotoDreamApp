using MyPhotoDreamApp.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPhotoDreamApp.Domain.ViewModels.Product
{
	public class ProductListViewModel
	{
		public int Id { get; set; }
		public List<MyPhotoDreamApp.Domain.Entity.Product> ProductList { get; set; }
		public string NewName { get; set; }
		public string NewDescription { get; set; }
		public decimal Price { get; set; }
		public CategoryProduct NewCategory { get; set; }
	}
}
