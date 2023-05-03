using Microsoft.AspNetCore.Mvc;
using MyPhotoDreamApp.DAL.Repositories;
using MyPhotoDreamApp.Domain.ViewModels.Order;
using MyPhotoDreamApp.Service.Interfaces;

namespace MyPhotoDreamApp.Controllers
{
	public class OrderController: Controller
	{

        private readonly IProductService _productService;
        private readonly ICategoryProductService _categoryProductService;

        public OrderController(IProductService productService, ICategoryProductService categoryProductService )
        {
            _productService = productService;
            _categoryProductService = categoryProductService;
        }
        [HttpGet]
        public async Task<ActionResult> CreateOrder(int id)
        {
            var response = await _productService.GetProduct(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                UploadFilesViewModel uploadFilesViewModel = new UploadFilesViewModel()
                {
                    Product = response.Data
                };
                return View(response.Data);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrder(IFormFileCollection uploads)
        {
            string numberOrder = "000458";
            var uploadPath = $"{Directory.GetCurrentDirectory()}/uploads";
            // создаем папку для хранения файлов
            Directory.CreateDirectory(uploadPath);
            uploadPath = $"{uploadPath}/{numberOrder}";
            Directory.CreateDirectory(uploadPath);
            foreach (var file in uploads)
            {
                // путь к папке uploads
                string untrustedFileName = Path.GetFileName($"{uploadPath}/{file.FileName}");
                string fullPath = $"{uploadPath}/{untrustedFileName}";

                // сохраняем файл в папку uploads
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }

            return RedirectToAction("Index", "Home");
        }

    }



    
}
