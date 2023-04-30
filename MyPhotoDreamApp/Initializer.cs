using MyPhotoDreamApp.DAL.Interfaces;
using MyPhotoDreamApp.DAL.Repositories;
using MyPhotoDreamApp.Domain.Entity;
using MyPhotoDreamApp.Service.Implementations;
using MyPhotoDreamApp.Service.Interfaces;

namespace MyPhotoDreamApp
{
    public static class Initializer
    {
        public static void InitializeRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBaseRepository<User>, UserRepository>();
			services.AddScoped<IBaseRepository<CategoryProduct>, CategoryRepository>();
            services.AddScoped<IBaseRepository<Product>, ProductRepository>();
		}

        public static void InitializeServices(this IServiceCollection services)
        {
			services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICategoryProductService, CategoryProductService>();
            services.AddScoped<IProductService, ProductService>();
		}
    }
}
