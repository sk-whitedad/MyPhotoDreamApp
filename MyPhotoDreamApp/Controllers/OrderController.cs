using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.CodeAnalysis;
using MyPhotoDreamApp.Domain.Entity;
using MyPhotoDreamApp.Domain.ViewModels.Order;
using MyPhotoDreamApp.Models;
using MyPhotoDreamApp.Service.Interfaces;
using System.Diagnostics;

namespace MyPhotoDreamApp.Controllers
{
    [Authorize]
	public class OrderController: Controller
	{
        private readonly IProductService _productService;
        private readonly ICategoryProductService _categoryProductService;
        private readonly IOrderService _orderService;
        private readonly IConfirmOrderService _confirmOrderService;
        private readonly IAccountService _accountService;

        public OrderController(IAccountService accountService, IConfirmOrderService confirmOrderService, IProductService productService, ICategoryProductService categoryProductService, IOrderService orderService )
        {
            _productService = productService;
            _categoryProductService = categoryProductService;
            _orderService = orderService;
            _confirmOrderService = confirmOrderService;
			_accountService = accountService;

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

		
		[HttpGet]//не доделан
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


        [HttpPost]
        public async Task<IActionResult> CreateConfirmOrder(string phoneNumber, string finalSumm1, bool checkDelivery, string deliveryAddress)
        {
            if (finalSumm1 == null)
            {
                return RedirectToAction("Index", "Home");
			}

            if (checkDelivery == false)
            {
                deliveryAddress = "";
			}
 			var confirmOrder = new ConfirmOrder()
            {
				SummOrder = Convert.ToDecimal(finalSumm1),
                DeliveryAddress = deliveryAddress,
                CheckDelivery = checkDelivery,
			};

            var responseConfirmOrderCreate = await _confirmOrderService.Create(confirmOrder, phoneNumber);
            if (responseConfirmOrderCreate.StatusCode == Domain.Enum.StatusCode.OK)
            {
                var confirmOrderViewModel = new ConfirmOrderViewModel()
                {
                    Id = responseConfirmOrderCreate.Data.Id,
                    PhoneNumber = User.Identity.Name,
                    FinalSumm = Convert.ToDecimal(finalSumm1)
				};
				return View(confirmOrderViewModel);
			}
			return RedirectToAction("Index", "Home");
		}

		
		[HttpGet]//для админа показывает все подтвержденные заказы
		[Authorize(Roles = "Admin")]
		public async Task<ActionResult> GetAllConfirmOrders()
		{
            
            var confirmOrderResponse = await _confirmOrderService.GetAllConfirmOrders();
            if (confirmOrderResponse.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(confirmOrderResponse.Data);
            }
			return RedirectToAction("Error");
		}


		
		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DelConfirmOrder(int id)
        {
            var confirmOrderResponse = _confirmOrderService.GetOrder(id);
            if (confirmOrderResponse.StatusCode == Domain.Enum.StatusCode.OK)
            {
                var ordersList = _confirmOrderService.GetListOrders(id).Data;
                var responseDelBdOrder = await _confirmOrderService.Delete(id);//удаление заказа из БД
                if (responseDelBdOrder.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    foreach (var order in ordersList)
                    {
                        await _orderService.Delete(order.Id);
                        _orderService.RemoveFolderOrder(order.Name);//удаление заказа из папки
                    }
                    return RedirectToAction("GetAllConfirmOrders", "Order");
                }
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
