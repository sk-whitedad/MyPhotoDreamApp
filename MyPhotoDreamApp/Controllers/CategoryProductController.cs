using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPhotoDreamApp.Domain.Entity;
using MyPhotoDreamApp.Domain.ViewModels.Category;
using MyPhotoDreamApp.Service.Interfaces;

namespace MyPhotoDreamApp.Controllers
{
	public class CategoryProductController : Controller
	{
		private readonly ICategoryProductService _categoryService;

		public CategoryProductController(ICategoryProductService categoryService)
		{
			_categoryService = categoryService;
		}

		[HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetCategories()
		{
			//CategoryProduct/GetCategories
			var response = _categoryService.GetCategories();
			var categoryList = new CategoriesListViewModel()
			{
				CategoryList = response.Data
			};
			if (response.StatusCode == Domain.Enum.StatusCode.OK)
			{
				return View(categoryList);
			}

			return RedirectToAction("Index", "Home");
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> AddCategory(CategoriesListViewModel categoryList)
		{
			var response = _categoryService.GetCategories();
			if (response.StatusCode == Domain.Enum.StatusCode.OK)
			{
				var category = new CategoryViewModel()
				{
					Name = categoryList.NewName,
					Description = categoryList.NewDescription
				};
				await _categoryService.Create(category);
				return RedirectToAction("GetCategories", "CategoryProduct");
			}

			return View("Ошибка записи");
		}


		[HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetCategory(int id)
		{
			var response = await _categoryService.GetCategory(id);
			if (response.StatusCode == Domain.Enum.StatusCode.OK)
			{
				return View(response.Data);
			}

			return RedirectToAction("GetCategories", "CategoryProduct");
		}

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetCategory(int id, CategoryViewModel category)
        {
            var response = await _categoryService.GetCategory(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
				category.Id = id;
				await _categoryService.Edit(id, category);
                return RedirectToAction("GetCategories", "CategoryProduct");
            }

            return View("Ошибка записи");
        }

		
		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DellCategories(int id)
		{
			var response = _categoryService.GetCategories();
			if (response.StatusCode == Domain.Enum.StatusCode.OK)
			{
				var category = response.Data.FirstOrDefault(x => x.Id == id);
				if (category != null)
				{
					await _categoryService.DeleteCategory(category.Id);
					return RedirectToAction("GetCategories", "CategoryProduct");
				}
				return View("Ошибка удаления");
			}
			return View("Ошибка удаления");
		}

	}
}
