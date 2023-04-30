using Microsoft.EntityFrameworkCore;
using MyPhotoDreamApp.DAL.Interfaces;
using MyPhotoDreamApp.Domain.Entity;
using MyPhotoDreamApp.Domain.Enum;
using MyPhotoDreamApp.Domain.Response;
using MyPhotoDreamApp.Domain.ViewModels.Product;
using MyPhotoDreamApp.Service.Interfaces;

namespace MyPhotoDreamApp.Service.Implementations
{
	public class ProductService : IProductService
	{
		private readonly IBaseRepository<Product> _productRepository;

		public ProductService(IBaseRepository<Product> productRepository)
		{
			_productRepository = productRepository;
		}

		public async Task<IBaseResponse<Product>> Create(ProductListViewModel model)
		{
			try
			{
				var product = new Product()
				{
					Name = model.NewName,
					Description = model.NewDescription,
					Price = model.Price,
					Category = model.NewCategory
				};
				await _productRepository.Create(product);
				return new BaseResponse<Product>()
				{
					StatusCode = StatusCode.OK,
					Data = product
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<Product>()
				{
					Description = $"[Create] : {ex.Message}",
					StatusCode = StatusCode.InternalServerError
				};
			}
		}

		public async Task<IBaseResponse<bool>> DeleteProducts(int id)
		{
			try
			{
				var product = await _productRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
				if (product == null)
				{
					return new BaseResponse<bool>()
					{
						Data = false,
						StatusCode = StatusCode.CarNotFound,
						Description = "Продукт не найден"
					};
				}
				await _productRepository.Delete(product);
				return new BaseResponse<bool>()
				{
					StatusCode = StatusCode.OK,
					Data = true
				};

			}
			catch (Exception ex)
			{
				return new BaseResponse<bool>
				{
					Description = $"[DeleteProducts] : {ex.Message}",
					StatusCode = StatusCode.InternalServerError
				};
			}
		}

		public async Task<IBaseResponse<Product>> Edit(int id, ProductListViewModel model)
		{
			try
			{
				var product = await _productRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
				if (product == null)
				{
					return new BaseResponse<Product>()
					{
						StatusCode = StatusCode.CarNotFound,
						Description = "Продукт не найден"
					};
				}
				Product _model = new Product()
				{
					Id = model.Id,
					Name = model.NewName,
					Description = model.NewDescription,
					Category = model.NewCategory
				};
				await _productRepository.Update(_model);
				return new BaseResponse<Product>()
				{
					StatusCode = StatusCode.OK,
					Data = _model
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<Product>()
				{
					Description = ex.Message,
					StatusCode = StatusCode.InternalServerError
				};
			}
		}

		public async Task<IBaseResponse<Product>> GetProduct(int id)
		{
			try
			{
				var product = await _productRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
				if (product == null)
				{
					return new BaseResponse<Product>()
					{
						StatusCode = StatusCode.CarNotFound,
						Description = "Продукт не найден"
					};
				}
				return new BaseResponse<Product>()
				{
					StatusCode = StatusCode.OK,
					Data = product
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<Product>()
				{
					StatusCode = StatusCode.InternalServerError,
					Description = $"[GetProduct]: {ex.Message}"
				};
			}
		}

		public IBaseResponse<List<Product>> GetProducts()
		{
			try
			{
				var products = _productRepository.GetAll().ToList();
				return new BaseResponse<List<Product>>()
				{
					StatusCode = StatusCode.OK,
					Data = products
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<List<Product>>()
				{
					StatusCode = StatusCode.InternalServerError,
					Description = $"[GetProducts] : {ex.Message}"
				};
			}
		}
	}
}
