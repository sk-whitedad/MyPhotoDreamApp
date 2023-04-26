using MyPhotoDreamApp.DAL.Interfaces;
using MyPhotoDreamApp.Domain.Entity;

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
            var category = _db.CategoryProducts.FirstOrDefault(x => x.Id == entity.Id);
            if (category != null)
            {
                category.Name = entity.Name;
                category.Description = entity.Description;
                await _db.SaveChangesAsync();
                return category;
            }
            return category;
        }
    }
}
