using Microsoft.EntityFrameworkCore;
using MyPhotoDreamApp.DAL.Interfaces;
using MyPhotoDreamApp.Domain.Entity;
using MyPhotoDreamApp.Domain.Enum;
using MyPhotoDreamApp.Domain.Response;
using MyPhotoDreamApp.Domain.ViewModels.Order;
using MyPhotoDreamApp.Service.Interfaces;

namespace MyPhotoDreamApp.Service.Implementations
{
    public class BasketService : IBasketService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<Product> _productRepository;
        private readonly IBaseRepository<CategoryProduct> _categoryProductRepository;

        public BasketService(IBaseRepository<User> userRepository, IBaseRepository<Product> productRepository, IBaseRepository<CategoryProduct> categoryRepository)
        {
            _userRepository = userRepository;
            _productRepository = productRepository;
            _categoryProductRepository = categoryRepository;
        }

        public async Task<IBaseResponse<IEnumerable<OrderViewModel>>> GetItems(string phoneNumber)
        {
            try
            {
                var products = _productRepository.GetAll()
                    .Include(p => p.Category)
                    .ToList();
                var catedories = _categoryProductRepository.GetAll().ToList();
                var user = await _userRepository.GetAll()
                .Include(x => x.Basket)
                .ThenInclude(x => x.Orders)
                .FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);


                if (user == null)
                {
                    return new BaseResponse<IEnumerable<OrderViewModel>>()
                    {
                        Description = "Пользователь не найден",
                        StatusCode = StatusCode.UserNotFound
                    };
                }

                var orders = user.Basket?.Orders;

                var response = from p in orders
                               join c in _productRepository.GetAll() on p.ProductId equals c.Id
                               select new OrderViewModel()
                               {
                                   Id = p.Id,
                                   Name = c.Name,
                                   CategoryName = c.Category.Name,
                                   Price = c.Price,
                                   Quantity = p.Quantity,
                                   DateCreated = p.DateCreated.ToLongDateString(),
                                   ProductName = c.Name
                               };

                return new BaseResponse<IEnumerable<OrderViewModel>>()
                {
                    Data = response,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<OrderViewModel>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<OrderViewModel>> GetItem(string phoneNumber, int id)
        {
            try
            {
                var user = await _userRepository.GetAll()
    .Include(x => x.Basket)
    .ThenInclude(x => x.Orders)
    .FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);

                if (user == null)
                {
                    return new BaseResponse<OrderViewModel>()
                    {
                        Description = "Пользователь не найден",
                        StatusCode = StatusCode.UserNotFound
                    };
                }

                var orders = user.Basket?.Orders.Where(x => x.Id == id).ToList();
                if (orders == null || orders.Count == 0)
                {
                    return new BaseResponse<OrderViewModel>()
                    {
                        Description = "Заказов нет",
                        StatusCode = StatusCode.OrderNotFound
                    };
                }

                var response = (from p in orders
                                join c in _productRepository.GetAll() on p.ProductId equals c.Id
                                select new OrderViewModel()
                                {
                                    Id = p.Id,
                                    ProductName = c.Name,
                                    CategoryName = c.Category.Name,
                                    Price = c.Price,
                                    DateCreated = p.DateCreated.ToLongDateString(),
                                }).FirstOrDefault();

                return new BaseResponse<OrderViewModel>()
                {
                    Data = response,
                    StatusCode = StatusCode.OK
                };



            }
            catch (Exception ex)
            {
                return new BaseResponse<OrderViewModel>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }

        }





    }
}
