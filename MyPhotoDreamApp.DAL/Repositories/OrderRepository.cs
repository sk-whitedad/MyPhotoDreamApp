using MyPhotoDreamApp.DAL.Interfaces;
using MyPhotoDreamApp.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPhotoDreamApp.DAL.Repositories
{
	public class OrderRepository : IBaseRepository<Order>
	{
		private readonly ApplicationDbContext _dbContext;

		public OrderRepository(ApplicationDbContext context)
		{
			_dbContext = context;
		}

		public async Task Create(Order entity)
		{
			await _dbContext.Orders.AddAsync(entity);
			await _dbContext.SaveChangesAsync();
		}

		public async Task Delete(Order entity)
		{
			_dbContext.Orders.Remove(entity);
			await _dbContext.SaveChangesAsync();
		}

		public IQueryable<Order> GetAll()
		{
			return _dbContext.Orders;
		}

		public async Task<Order> Update(Order entity)
		{
			_dbContext.Orders.Update(entity);
			await _dbContext.SaveChangesAsync();
			return entity;
		}

		
	}
}
