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
                var order = _orderRepository.GetAll()
                     .Select(x => x.Basket.Orders.FirstOrDefault(y => y.Id == id))
                     .FirstOrDefault();

                if (order == null)
                {
                    return new BaseResponse<bool>()
                    {
                        StatusCode = StatusCode.OrderNotFound,
                        Description = "Заказ не найден"
                    };
                }

                await _orderRepository.Delete(order);
                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.OK,
                    Description = "Заказ удален"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
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
    }
}
