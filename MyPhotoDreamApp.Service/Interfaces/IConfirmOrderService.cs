using MyPhotoDreamApp.Domain.Entity;
using MyPhotoDreamApp.Domain.Response;

namespace MyPhotoDreamApp.Service.Interfaces
{
	public interface IConfirmOrderService
	{
		Task<IBaseResponse<ConfirmOrder>> Create(ConfirmOrder model, string phoneNumber);

		Task<IBaseResponse<bool>> Delete(int id);

		Task<IBaseResponse<int>> GetCount();

		IBaseResponse<ConfirmOrder> GetOrder(int id);
	}
}
