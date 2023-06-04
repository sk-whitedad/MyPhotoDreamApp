using Microsoft.EntityFrameworkCore;
using MyPhotoDreamApp.DAL.Interfaces;
using MyPhotoDreamApp.DAL.Repositories;
using MyPhotoDreamApp.Domain.Entity;
using MyPhotoDreamApp.Domain.Enum;
using MyPhotoDreamApp.Domain.Response;
using MyPhotoDreamApp.Domain.ViewModels.Order;
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
				var confirmOrder = _confirmOrderRepository.GetAll()
					.FirstOrDefault(y => y.Id == id);

				if (confirmOrder == null)
				{
                    return new BaseResponse<bool>()
                    {
                        Description = "Заказ не найден",
                        StatusCode = StatusCode.UserNotFound
                    };
                }
				await _confirmOrderRepository.Delete(confirmOrder);

                return new BaseResponse<bool>()
				{
					Data = true,
                    Description = "Заказ удален",
                    StatusCode = StatusCode.OK
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

		public async Task<IBaseResponse<List<AllConfirmOrderViewModel>>> GetAllConfirmOrders()
		{
			try
			{ 
				var allConfirmOrders = _confirmOrderRepository.GetAll()
					.Include(x => x.User)
					.ToList();
				
				if (allConfirmOrders == null)
				{
                    return new BaseResponse<List<AllConfirmOrderViewModel>>()
                    {
                        Description = "Подтвержденные заказы не найдены",
                        StatusCode = StatusCode.UserNotFound
                    };
                }
                var _allConfirmOrdersList = new List<AllConfirmOrderViewModel>();
                foreach (var confirmOrder in allConfirmOrders)
				{
					string chkD;
					if (confirmOrder.CheckDelivery == true)
						chkD = "Есть";
					else chkD = "Нет";
					var allConfirmOrderViewModel = new AllConfirmOrderViewModel()
					{
						Id = confirmOrder.Id,
                        SummOrder = confirmOrder.SummOrder,
                        CheckDelivery = chkD,
						DeliveryAddress = confirmOrder.DeliveryAddress,
						DateCreated = confirmOrder.DateCreated,
						PhoneNumber = confirmOrder.User.PhoneNumber,
                    };

                    _allConfirmOrdersList.Add(allConfirmOrderViewModel);
                }
				
                return new BaseResponse<List<AllConfirmOrderViewModel>>()
				{
					Data = _allConfirmOrdersList,
                    Description = "Подтвержденные заказы найдены",
                    StatusCode = StatusCode.OK
                };
			}
			catch (Exception ex)
			{
				return new BaseResponse<List<AllConfirmOrderViewModel>>()
				{
					Description = ex.Message,
					StatusCode = StatusCode.InternalServerError
				};
			}
		}

        public IBaseResponse<List<Order>> GetListOrders(int id)
        {
            try
            {
				var confirmOrder = _confirmOrderRepository.GetAll()
					.Include(x => x.Orders)
					.FirstOrDefault(y => y.Id == id);
				if (confirmOrder == null)
				{
                    return new BaseResponse<List<Order>>()
                    {
                        Description = "Подтвержденные заказы не найдены",
                        StatusCode = StatusCode.UserNotFound
                    };
                }

                return new BaseResponse<List<Order>>()
                {
                    Data = confirmOrder.Orders,
                    Description = "Звказы найдены",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Order>>()
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
				var confirmOrder = _confirmOrderRepository.GetAll()
					.FirstOrDefault(y => y.Id == id);
				if (confirmOrder == null)
				{
					return new BaseResponse<ConfirmOrder>()
					{
						Description = "Подтвержденный заказ не найден",
						StatusCode = StatusCode.UserNotFound
					};
				}
				return new BaseResponse<ConfirmOrder>()
				{
					Data = confirmOrder,
					StatusCode= StatusCode.OK,
					Description = "Заказ найден"
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



		public IBaseResponse<bool> ClearZipFolder(string path)
		{
			var uploadPath = $"{Directory.GetCurrentDirectory()}/{path}";

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
					Data = false,
					Description = ex.Message,
					StatusCode = StatusCode.InternalServerError
				};
			}
		}



	}
}
