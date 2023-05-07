using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.FileProviders;
using MyPhotoDreamApp.DAL.Repositories;
using MyPhotoDreamApp.Domain.Entity;
using MyPhotoDreamApp.Domain.ViewModels.Order;
using MyPhotoDreamApp.Service.Interfaces;
using System.ComponentModel;
using System.Web.Helpers;

namespace MyPhotoDreamApp.Controllers
{
	public class OrderController: Controller
	{
        private readonly IProductService _productService;
        private readonly ICategoryProductService _categoryProductService;
        private readonly IOrderService _orderService;

        public OrderController(IProductService productService, ICategoryProductService categoryProductService, IOrderService orderService )
        {
            _productService = productService;
            _categoryProductService = categoryProductService;
            _orderService = orderService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> CreateOrder(int id)
        {
            var response = await _productService.GetProduct(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                 return View(response.Data);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> CreateOrder(IFormFileCollection uploads, int id, List<int> idInputs)
        {
            var responseProduct = await _productService.GetProduct(id);
            var responseOrder = await _orderService.GetCount();
            if (responseProduct.StatusCode == Domain.Enum.StatusCode.OK && responseOrder.StatusCode == Domain.Enum.StatusCode.OK)
            {
                string numberOrder = $"order_{responseOrder.Data}_{responseProduct.Data.Name}";
                var uploadPath = $"{Directory.GetCurrentDirectory()}/uploads";
                // создаем папку для хранения файлов
                Directory.CreateDirectory(uploadPath);
                uploadPath = $"{uploadPath}/{numberOrder}";
                Directory.CreateDirectory(uploadPath);
                int i = 0;
                foreach (var file in uploads)
                {
                    // путь к папке uploads
                    i++;
                    string untrustedFileName = Path.GetFileName($"{uploadPath}/{i}_{responseProduct.Data.Name}_{idInputs[i-1]}");
                    string fullPath = $"{uploadPath}/{untrustedFileName}";

                    // сохраняем файл в папку uploads
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }

                //формируем объект заказа 
                Order order = new Order()
                {
                    Name = numberOrder,
                    PhoneNumber = HttpContext.User.Identity.Name,
                    Quantity = idInputs.Sum(),
                    Price = responseProduct.Data.Price,
                    DateCreated = DateTime.Now,
                    ProductId = responseProduct.Data.Id,
                };
                await _orderService.Create(order);
                return RedirectToAction("Detail", "Basket");
            }
            return RedirectToAction("Index", "Home");
        }

 

    }



    
}
