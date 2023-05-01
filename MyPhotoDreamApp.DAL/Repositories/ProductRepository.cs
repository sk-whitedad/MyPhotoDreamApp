using Microsoft.EntityFrameworkCore;
using MyPhotoDreamApp.DAL.Interfaces;
using MyPhotoDreamApp.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPhotoDreamApp.DAL.Repositories
{
	public class ProductRepository : IBaseRepository<Product>
	{
		private readonly ApplicationDbContext _db;

		public ProductRepository(ApplicationDbContext db)
		{
			_db = db;
		}

		public async Task Create(Product entity)
		{
			await _db.Products.AddAsync(entity);
			await _db.SaveChangesAsync();
		}

		public async Task Delete(Product entity)
		{
			_db.Products.Remove(entity);
			await _db.SaveChangesAsync();
		}

		public IQueryable<Product> GetAll()
		{
			return _db.Products;
		}

		public async Task<Product> Update(Product entity)
		{
			var product = await _db.Products.FirstOrDefaultAsync(x => x.Id == entity.Id);
			if (product != null)
			{
				product.Name = entity.Name;
				product.Description = entity.Description;
				product.Price = entity.Price;
				product.Category = entity.Category;
				await _db.SaveChangesAsync();
			}
			return product;
		}
	}
}
