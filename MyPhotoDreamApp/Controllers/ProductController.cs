using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyPhotoDreamApp.Domain.ViewModels.Product;
using MyPhotoDreamApp.Service.Interfaces;

namespace MyPhotoDreamApp.Controllers
{
	public class ProductController : Controller
	{
		private readonly IProductService _productService;
		private readonly ICategoryProductService _categoryProductService;

		public ProductController(IProductService productService, ICategoryProductService categoryProductService)
		{
			_productService = productService;
			_categoryProductService = categoryProductService;
		}

		[HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetProducts()
		{
			var responseCategory = _categoryProductService.GetCategories();
			var responseProduct = _productService.GetProducts();
			var productList = new ProductListViewModel()
			{
				ProductList = responseProduct.Data
			};
			if (responseProduct.StatusCode == Domain.Enum.StatusCode.OK && responseCategory.StatusCode == Domain.Enum.StatusCode.OK)
			{
				List<string> sourse = new List<string>();
				foreach (var item in responseCategory.Data)
				{
					sourse.Add(item.Name);
				}
				SelectList selectList = new SelectList(sourse, sourse[0]);
				ViewBag.SelectItems = selectList;
				return View(productList);
			}

			return RedirectToAction("Index", "Home");
		}



		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> AddNewProduct(string SelectedItem, ProductListViewModel productList)
		{
			var responseCategory = _categoryProductService.GetCategories();
			var responseProduct = _productService.GetProducts();
			if (responseProduct.StatusCode == Domain.Enum.StatusCode.OK && responseCategory.StatusCode == Domain.Enum.StatusCode.OK)
			{
				var category = responseCategory.Data.FirstOrDefault(x => x.Name == SelectedItem);
				var product = new ProductListViewModel()
				{
					NewName = productList.NewName,
					NewDescription = productList.NewDescription,
					Price = productList.Price,
					NewCategory = category,
				};
				await _productService.Create(product);
				return RedirectToAction("GetProducts", "Product");
			}

			return View("Ошибка записи");
		}


	}
}
