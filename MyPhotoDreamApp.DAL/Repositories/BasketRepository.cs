using MyPhotoDreamApp.DAL.Interfaces;
using MyPhotoDreamApp.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPhotoDreamApp.DAL.Repositories
{
    public class BasketRepository : IBaseRepository<Basket>
    {
        private readonly ApplicationDbContext _dbContext;

        public BasketRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Basket> GetAll()
        {
            return _dbContext.Baskets;
        }

        public Task Delete(Basket entity)
        {
            throw new NotImplementedException();
        }

        public Task<Basket> Update(Basket entity)
        {
            throw new NotImplementedException();
        }

        public async Task Create(Basket entity)
        {
            await _dbContext.Baskets.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
