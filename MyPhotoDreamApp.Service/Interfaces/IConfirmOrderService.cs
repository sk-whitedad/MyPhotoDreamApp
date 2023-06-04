using MyPhotoDreamApp.Domain.Entity;
using MyPhotoDreamApp.Domain.Response;
using MyPhotoDreamApp.Domain.ViewModels.Order;

namespace MyPhotoDreamApp.Service.Interfaces
{
	public interface IConfirmOrderService
	{
		Task<IBaseResponse<ConfirmOrder>> Create(ConfirmOrder model, string phoneNumber);

		Task<IBaseResponse<bool>> Delete(int id);

		Task<IBaseResponse<List<AllConfirmOrderViewModel>>> GetAllConfirmOrders();

		IBaseResponse<ConfirmOrder> GetOrder(int id);

		IBaseResponse<List<Order>> GetListOrders(int id);

		IBaseResponse<bool> ClearZipFolder(string path);

	}
}
