using Microsoft.EntityFrameworkCore;
using MyPhotoDreamApp.DAL.Interfaces;
using MyPhotoDreamApp.Domain.Entity;

namespace MyPhotoDreamApp.DAL.Repositories
{
    public class UserRepository : IBaseRepository<User>
    {
        private readonly ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task Create(User entity)
        {
            await _db.Users.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(User entity)
        {
           _db.Users.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public IQueryable<User> GetAll()
        {
            return _db.Users;
        }

        public async Task<User> Update(User entity)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (user != null)
            {
                user.PhoneNumber = entity.PhoneNumber;
                user.Role = entity.Role;
                if (entity.Password != null)
                {
                    user.Password = entity.Password;
                }
                await _db.SaveChangesAsync();
            }
            return user;
        }
    }
}
