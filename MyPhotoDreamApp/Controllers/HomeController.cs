using Microsoft.AspNetCore.Mvc;
using MyPhotoDreamApp.Domain.ViewModels.Home;
using MyPhotoDreamApp.Models;
using MyPhotoDreamApp.Service.Interfaces;
using System.Diagnostics;

namespace MyPhotoDreamApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryProductService _categoryProductService;

        public HomeController(IProductService productService, ICategoryProductService categoryProductService)
        {
            _productService = productService;
            _categoryProductService = categoryProductService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var responseProduct = _productService.GetProducts();
            if (responseProduct.StatusCode == Domain.Enum.StatusCode.OK)
            {
                var productList = new ProductListViewModel()
                {
                    Products = responseProduct.Data
                };

                return View(productList);
            }
            return View("Ошибка загрузки");
        }

        public IActionResult Contacts()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}