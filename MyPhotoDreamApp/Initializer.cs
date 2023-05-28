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
            services.AddScoped<IBaseRepository<Order>, OrderRepository>();
            services.AddScoped<IBaseRepository<Basket>, BasketRepository>();
            services.AddScoped<IBaseRepository<ConfirmOrder>, ConfirmOrderRepository>();
		}

        public static void InitializeServices(this IServiceCollection services)
        {
			services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICategoryProductService, CategoryProductService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IConfirmOrderService, ConfirmOrderService>();
		}
    }
}
