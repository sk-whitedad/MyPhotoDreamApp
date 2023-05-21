using MyPhotoDreamApp.Domain.Entity;
using MyPhotoDreamApp.Domain.Response;
using MyPhotoDreamApp.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPhotoDreamApp.Service.Implementations
{
	public class ConfirmOrderService : IConfirmOrderService
	{
		public Task<IBaseResponse<ConfirmOrder>> Create(ConfirmOrder model)
		{
			throw new NotImplementedException();
		}

		public Task<IBaseResponse<bool>> Delete(int id)
		{
			throw new NotImplementedException();
		}

		public Task<IBaseResponse<int>> GetCount()
		{
			throw new NotImplementedException();
		}

		public IBaseResponse<ConfirmOrder> GetOrder(int id)
		{
			throw new NotImplementedException();
		}
	}
}
