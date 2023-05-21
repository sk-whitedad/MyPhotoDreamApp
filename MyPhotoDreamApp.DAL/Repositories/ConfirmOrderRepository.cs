using MyPhotoDreamApp.DAL.Interfaces;
using MyPhotoDreamApp.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPhotoDreamApp.DAL.Repositories
{
	internal class ConfirmOrderRepository : IBaseRepository<ConfirmOrder>
	{
		private readonly ApplicationDbContext _dbContext;

		public ConfirmOrderRepository(ApplicationDbContext context)
		{
			_dbContext = context;
		}

		public async Task Create(ConfirmOrder entity)
		{
			await _dbContext.ConfirmOrders.AddAsync(entity);
			await _dbContext.SaveChangesAsync();
		}

		public async Task Delete(ConfirmOrder entity)
		{
			_dbContext.ConfirmOrders.Remove(entity);
			await _dbContext.SaveChangesAsync();
		}

		public IQueryable<ConfirmOrder> GetAll()
		{
			return _dbContext.ConfirmOrders;
		}

		public async Task<ConfirmOrder> Update(ConfirmOrder entity)
		{
			_dbContext.ConfirmOrders.Update(entity);
			await _dbContext.SaveChangesAsync();
			return entity;
		}
	}
}
