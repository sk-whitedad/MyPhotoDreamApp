using MyPhotoDreamApp.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPhotoDreamApp.Domain.ViewModels.Category
{
	public class CategoriesListViewModel
	{
		public List<CategoryProduct> CategoryList { get; set; }
		public string NewName { get; set; }
		public string NewDescription { get; set; }
	}
}
