using MyPhotoDreamApp.DAL.Interfaces;
using MyPhotoDreamApp.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPhotoDreamApp.DAL.Repositories
{
    public class CategoryRepository : IBaseRepository<CategoryProduct>
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task Create(CategoryProduct entity)
        {
            await _db.CategoryProducts.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(CategoryProduct entity)
        {
            _db.CategoryProducts.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public IQueryable<CategoryProduct> GetAll()
        {
            return _db.CategoryProducts;
        }

        public async Task<CategoryProduct> Update(CategoryProduct entity)
        {
            _db.CategoryProducts.Update(entity);
            await _db.SaveChangesAsync();

            return entity;
        }
    }
}
