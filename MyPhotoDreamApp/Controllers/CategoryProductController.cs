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
			if (response.StatusCode == Domain.Enum.StatusCode.OK)
			{
				return PartialView(response.Data);
			}

			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetCategory(int id)
		{
			var response = await _categoryService.GetCategory(id);
			if (response.StatusCode == Domain.Enum.StatusCode.OK)
			{
				return PartialView(response.Data);
			}

			return RedirectToAction("GetCategories", "CategoryProduct");
		}

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetCategory(int id, CategoryProduct category)
        {
            var response = await _categoryService.GetCategory(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
				var _category = new CategoryViewModel()
				{
					Id = id,
					Name = category.Name,
					Description = category.Description,
				};

				_categoryService.Edit(id, _category);
                return RedirectToAction("GetCategories", "CategoryProduct");
            }

            return View("Ошибка записи");
        }


    }
}
