using MyPhotoDreamApp.Domain.Entity;
using MyPhotoDreamApp.Domain.Response;
using MyPhotoDreamApp.Domain.ViewModels.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPhotoDreamApp.Service.Interfaces
{
    public interface IOrderService
    {
        Task<IBaseResponse<Order>> Create(Order model);

        Task<IBaseResponse<bool>> Delete(int id);

        Task<IBaseResponse<int>> GetCount();

        IBaseResponse<bool> RemoveFolderOrder(string orderName);

        IBaseResponse<Order> GetOrder(int id);
	}
}
