using Microsoft.EntityFrameworkCore;
using MyPhotoDreamApp.DAL.Interfaces;
using MyPhotoDreamApp.DAL.Repositories;
using MyPhotoDreamApp.Domain.Entity;
using MyPhotoDreamApp.Domain.Enum;
using MyPhotoDreamApp.Domain.Response;
using MyPhotoDreamApp.Service.Interfaces;

namespace MyPhotoDreamApp.Service.Implementations
{
	public class ConfirmOrderService : IConfirmOrderService
	{
		private readonly IBaseRepository<User> _userRepository;
		private readonly IBaseRepository<ConfirmOrder> _confirmOrderRepository;
		private IBaseRepository<Order> _orderRepository;

		public ConfirmOrderService(IBaseRepository<Order> orderRepository, IBaseRepository<User> userRepository, IBaseRepository<ConfirmOrder> confirmOrderRepository)
		{
			_userRepository = userRepository;
			_confirmOrderRepository = confirmOrderRepository;
			_orderRepository = orderRepository;
		}

		public async Task<IBaseResponse<ConfirmOrder>> Create(ConfirmOrder model, string phoneNumber)
		{
			try
			{
				var user = await _userRepository.GetAll()
				.Include(x => x.Basket)
				.ThenInclude(y => y.Orders)
				.FirstOrDefaultAsync(z => z.PhoneNumber == phoneNumber);

				if (user == null)
				{
					return new BaseResponse<ConfirmOrder>()
					{
						Description = "Пользователь не найден",
						StatusCode = StatusCode.UserNotFound
					};
				}

				var orders = _orderRepository.GetAll().Where(x => (x.PhoneNumber == phoneNumber && x.ConfirmOrderId == null)).ToList();
				if (orders == null)
				{
					return new BaseResponse<ConfirmOrder>()
					{
						Description = "Пользователь не найден",
						StatusCode = StatusCode.UserNotFound
					};
				}

				foreach (var order in orders) 
				{
					order.CheckConfirm = true;
					await _orderRepository.Update(order);
				}

				//Добавляем в Order ссылку на объект ConfirmOrder
				var confirmOrder = new ConfirmOrder()
				{
					SummOrder = model.SummOrder,
					DeliveryAddress = model.DeliveryAddress,
					CheckDelivery = model.CheckDelivery,
					Orders = orders,
					DateCreated = DateTime.Now,
					User = user,
					
				};

				await _confirmOrderRepository.Create(confirmOrder);

				return new BaseResponse<ConfirmOrder>()
				{
					Data = confirmOrder,
					StatusCode = StatusCode.OK,
					Description = "Заказ создан"
				};

			}
			catch (Exception ex)
			{
				return new BaseResponse<ConfirmOrder>()
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
				return new BaseResponse<bool>()
				{
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
				return new BaseResponse<int>()
				{
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

		public IBaseResponse<ConfirmOrder> GetOrder(int id)
		{
			try
			{
				return new BaseResponse<ConfirmOrder>()
				{
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<ConfirmOrder>()
				{
					Description = ex.Message,
					StatusCode = StatusCode.InternalServerError
				};
			}
		}
	}
}
