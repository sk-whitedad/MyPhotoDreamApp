using Microsoft.EntityFrameworkCore;
using MyPhotoDreamApp.DAL.Interfaces;
using MyPhotoDreamApp.Domain.Entity;
using MyPhotoDreamApp.Domain.Enum;
using MyPhotoDreamApp.Domain.Response;
using MyPhotoDreamApp.Domain.ViewModels.Order;
using MyPhotoDreamApp.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPhotoDreamApp.Service.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<Order> _orderRepository;

        public OrderService(IBaseRepository<User> userRepository, IBaseRepository<Order> orderRepository)
        {
            _userRepository = userRepository;
            _orderRepository = orderRepository;
        }

        public async Task<IBaseResponse<Order>> Create(Order model)
        {
            try
            {
                var user = await _userRepository.GetAll()
                    .Include(x => x.Basket)
                    .FirstOrDefaultAsync(x => x.PhoneNumber == model.PhoneNumber);
                if (user == null)
                {
                    return new BaseResponse<Order>()
                    {
                        Description = "Пользователь не найден",
                        StatusCode = StatusCode.UserNotFound
                    };
                }

                var order = new Order()
                {
                    Name = model.Name,
                    PhoneNumber = model.PhoneNumber,
                    DateCreated = model.DateCreated,
                    Quantity = model.Quantity,
                    Price = model.Price,
                    Basket = user.Basket,
                    ProductId = model.ProductId,
                };

                await _orderRepository.Create(order);

                return new BaseResponse<Order>()
                {
                    Description = "Заказ создан",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Order>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<bool>> Delete(int id)
        {
            try
            {
                var order = _orderRepository.GetAll().FirstOrDefault(x => x.Id == id);

                if (order == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.OrderNotFound,
                        Description = "Заказ не найден"
                    };
                }

                await _orderRepository.Delete(order);
                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = StatusCode.OK,
                    Description = "Заказ удален"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Data = false,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<int>> GetCount()
        {
            try
            {
                var orders = _orderRepository.GetAll();
                if (orders == null)
                {
                    return new BaseResponse<int>()
                    {
                        StatusCode = StatusCode.OrderNotFound,
                        Description = "Заказы не найдены"
                    };
                }
                return new BaseResponse<int>()
                {
                    Data = orders.Count(),
                    Description = "Количество получено",
                    StatusCode = StatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<int>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

		public IBaseResponse<Order> GetOrder(int id)
        {
            try
            {
                var orders = _orderRepository.GetAll();
                if (orders == null)
                {
                    return new BaseResponse<Order>()
                    {
                        StatusCode = StatusCode.OrderNotFound,
                        Description = "Заказы не найдены"
                    };
 				}
				var order = orders.FirstOrDefault(x => x.Id == id);
				return new BaseResponse<Order>()
				{
					Data = order,
					Description = "Заказ получен",
					StatusCode = StatusCode.OK
				};
			}
            catch (Exception ex)
            {
				return new BaseResponse<Order>()
				{
					Description = ex.Message,
					StatusCode = StatusCode.InternalServerError
				};
			}
        }

		public IBaseResponse<bool> RemoveFolderOrder(string orderName)
		{
			var uploadPath = $"{Directory.GetCurrentDirectory()}/uploads/{orderName}";

            if (!Directory.Exists(uploadPath)) 
            {
				return new BaseResponse<bool>()
				{
                    Data = false,
					StatusCode = StatusCode.OrderNotFound,
					Description = "Каталог заказа не найден"
				};
			};

			try 
			{
				Directory.Delete(uploadPath, true);
				return new BaseResponse<bool>()
				{
                    Data = true,
					Description = "Каталог заказа уделен",
					StatusCode = StatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<bool>()
				{
                    Data= false,
					Description = ex.Message,
					StatusCode = StatusCode.InternalServerError
				};
			}
		}
	}
}
