using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPhotoDreamApp.Domain.ViewModels.Product;
using MyPhotoDreamApp.Service.Interfaces;

namespace MyPhotoDreamApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetProducts()
        {
            var response = _productService.GetProducts();
            var productList = new ProductListViewModel()
            {
                ProductList = response.Data
            };
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(productList);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
