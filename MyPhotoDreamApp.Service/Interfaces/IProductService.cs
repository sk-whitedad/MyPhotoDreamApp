using MyPhotoDreamApp.Domain.Entity;
using MyPhotoDreamApp.Domain.Response;
using MyPhotoDreamApp.Domain.ViewModels.Category;
using MyPhotoDreamApp.Domain.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace MyPhotoDreamApp.Service.Interfaces
{
	public interface IProductService
	{
		IBaseResponse<List<Product>> GetProducts();

		Task<IBaseResponse<Product>> Create(ProductListViewModel model);

		Task<IBaseResponse<bool>> DeleteProducts(int id);

		Task<IBaseResponse<Product>> Edit(int id, Product model);

		Task<IBaseResponse<Product>> GetProduct(int id);
	}
}
