using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.FileProviders;
using MyPhotoDreamApp.DAL.Repositories;
using MyPhotoDreamApp.Domain.Entity;
using MyPhotoDreamApp.Domain.ViewModels.Order;
using MyPhotoDreamApp.Models;
using MyPhotoDreamApp.Service.Interfaces;
using NuGet.Packaging.Signing;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Web.Helpers;

namespace MyPhotoDreamApp.Controllers
{
    [Authorize]
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
        public async Task<ActionResult> CreateOrder(IFormFileCollection uploads, int id, List<int> idInputs)
        {
            var responseProduct = await _productService.GetProduct(id);
            var responseOrder = await _orderService.GetCount();
            if (responseProduct.StatusCode == Domain.Enum.StatusCode.OK && responseOrder.StatusCode == Domain.Enum.StatusCode.OK)
            {
                string numberOrder = $"order_{responseOrder.Data}_{responseProduct.Data.Name}";
                var uploadPath = $"{Directory.GetCurrentDirectory()}/uploads";
                // создаем папку для хранения файлов
                uploadPath = $"{uploadPath}/{numberOrder}";
                Directory.CreateDirectory(uploadPath);
                int i = 0;
                foreach (var file in uploads)
                {
					// путь к папке uploads
                    i++;
					var extension = Path.GetExtension(file.FileName);//получаем расширение файла
					string untrustedFileName = Path.GetFileName($"{uploadPath}/{i}_{responseProduct.Data.Name}_{idInputs[i-1]}{extension}");
                    string fullPath = $"{uploadPath}/{untrustedFileName}";


                    using(var image = Bitmap.FromStream(file.OpenReadStream()))
                    {
                        if (image != null)
                        {
                            Size s = image.Size;
                            int w = s.Width;
                            int h = s.Height;
                            float k = w / 200;
                            s.Width = 200;
                            s.Height = 200;
                            
                            var uploadPath2 = $"{Directory.GetCurrentDirectory()}/wwwroot/img/ImgOrdersPreview";
                            uploadPath2 = $"{uploadPath2}/{numberOrder}";
                            string untrustedFileName2 = Path.GetFileName($"{uploadPath2}/{i}_{responseProduct.Data.Name}_{idInputs[i - 1]}{extension}");
                            string fullPath2 = $"{uploadPath2}/{untrustedFileName2}";
                            Bitmap newImage = new Bitmap(image, s);

                            newImage.Save(fullPath2);
                        }
                    }
                    
                    // сохраняем файл в папку uploads
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
 
                    }
                }

                //формируем объект заказа для БД
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

		[HttpPost]
 		public async Task<IActionResult> DelOrder(int id)
		{
            var orderResponse = _orderService.GetOrder(id);
            if (orderResponse.StatusCode == Domain.Enum.StatusCode.OK)
            {
				var responseDelBdOrder = await _orderService.Delete(id);//удаление заказа из БД
                if (responseDelBdOrder.StatusCode == Domain.Enum.StatusCode.OK)
                {
					_orderService.RemoveFolderOrder(orderResponse.Data.Name);//удаление заказа из папки
					return RedirectToAction("Detail", "Basket");
				}
			}
            
			return RedirectToAction("Error");
		}

		
		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			var orderResponse = _orderService.GetOrder(id);
            
			if (orderResponse.StatusCode == Domain.Enum.StatusCode.OK)
			{
                var productResponse = await _productService.GetProduct(orderResponse.Data.ProductId);
                var categoryResponse = await _categoryProductService.GetCategory(productResponse.Data.CategoryProductId);
                OrderViewModel orderViewModel = new OrderViewModel()
                {
                    Id = orderResponse.Data.Id,
                    Name = orderResponse.Data.Name,
                    Quantity = orderResponse.Data.Quantity,
                    Price = orderResponse.Data.Price,
                    DateCreated = orderResponse.Data.DateCreated.ToLongDateString(),
                    ProductName = productResponse.Data.Name,
                    CategoryName = categoryResponse.Data.Name

                };

                return View(orderViewModel);
			}

			return RedirectToAction("Error");
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}


	}




}
