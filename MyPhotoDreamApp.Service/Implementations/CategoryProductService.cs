using Microsoft.EntityFrameworkCore;
using MyPhotoDreamApp.DAL.Interfaces;
using MyPhotoDreamApp.Domain.Entity;
using MyPhotoDreamApp.Domain.Enum;
using MyPhotoDreamApp.Domain.Response;
using MyPhotoDreamApp.Domain.ViewModels.Category;
using MyPhotoDreamApp.Service.Interfaces;

namespace MyPhotoDreamApp.Service.Implementations
{
	public class CategoryProductService : ICategoryProductService
	{
		private readonly IBaseRepository<CategoryProduct> _categoryRepository;

		public CategoryProductService(IBaseRepository<CategoryProduct> categoryRepository)
		{
			_categoryRepository = categoryRepository;
		}

		public async Task<IBaseResponse<CategoryProduct>> Create(CategoryViewModel model)
		{
			try
			{
				var category = new CategoryProduct()
				{
					Name = model.Name,
					Description = model.Description
				};

				await _categoryRepository.Create(category);
				return new BaseResponse<CategoryProduct>()
				{
					StatusCode = StatusCode.OK,
					Data = category
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<CategoryProduct>()
				{
					Description = ex.Message,
					StatusCode = StatusCode.InternalServerError
				};
			}
		}

		public async Task<IBaseResponse<bool>> DeleteCategory(int id)
		{
			try
			{
				var category = await _categoryRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
				if (category == null)
				{
					return new BaseResponse<bool>()
					{
						Data = false,
						StatusCode = StatusCode.CarNotFound,
						Description = "Категория не найдена"
					};
				}

				await _categoryRepository.Delete(category);
				return new BaseResponse<bool>()
				{
					StatusCode = StatusCode.OK,
					Data = true
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<bool>()
				{
					Description = $"[DeleteCar] : {ex.Message}",
					StatusCode = StatusCode.InternalServerError
				};
			}
		}

		public async Task<IBaseResponse<CategoryProduct>> Edit(int id, CategoryViewModel model)
		{
			try
			{
				var category = await _categoryRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
				if (category == null)
				{
					return new BaseResponse<CategoryProduct>()
					{
						StatusCode = StatusCode.CarNotFound,
						Description = "Категория не найдена"
					};
				}
				category.Name = model.Name;
				category.Description = model.Description;
				await _categoryRepository.Update(category);
				return new BaseResponse<CategoryProduct>()
				{
					StatusCode = StatusCode.OK,
					Data = category
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<CategoryProduct>()
				{
					Description = ex.Message,
					StatusCode = StatusCode.InternalServerError
				};

			}
		}

		public IBaseResponse<List<CategoryProduct>> GetCategories()
		{
			try
			{
				var categories = _categoryRepository.GetAll().ToList();
				return new BaseResponse<List<CategoryProduct>>()
				{
					StatusCode = StatusCode.OK,
					Data = categories
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<List<CategoryProduct>>()
				{
					StatusCode = StatusCode.InternalServerError,
					Description = $"[GetCategories] : {ex.Message}"
				};
			}
		}

		public async Task<IBaseResponse<CategoryProduct>> GetCategory(int id)
		{
			try
			{
				var category = await _categoryRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
				if (category == null)
				{
					return new BaseResponse<CategoryProduct>()
					{
						StatusCode = StatusCode.CarNotFound,
						Description = "Категория не найдена"
					};
				}

				return new BaseResponse<CategoryProduct>()
				{
					StatusCode = StatusCode.OK,
					Data = category
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<CategoryProduct>()
				{
					StatusCode = StatusCode.InternalServerError,
					Description = $"[GetCategory]: {ex.Message}"
				};
			}
		}
	}
}
