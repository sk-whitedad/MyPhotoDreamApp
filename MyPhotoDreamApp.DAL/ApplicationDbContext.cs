using Microsoft.EntityFrameworkCore;
using MyPhotoDreamApp.Domain.Entity;
using MyPhotoDreamApp.Domain.Enum;
using MyPhotoDreamApp.Domain.Helpers;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace MyPhotoDreamApp.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
		public DbSet<Basket> Baskets { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<CategoryProduct> CategoryProducts { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();    // создаем базу данных при первом обращении
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(builder =>
            {
                builder.ToTable("Users").HasKey(x => x.Id);
                builder.HasData(
                    new User
                    {
                        Id = 1,
                        PhoneNumber = "1234567890",
                        Password = HashPasswordHelper.HashPassowrd("123456"),
                        Role = Role.Admin
                    });
                builder.Property(x => x.Id).ValueGeneratedOnAdd();
                builder.Property(x => x.PhoneNumber).IsRequired();
                builder.Property(x => x.Password).IsRequired();
                builder.Property(x => x.Role).IsRequired();
            });

			modelBuilder.Entity<CategoryProduct>(builder =>
			{
				builder.ToTable("CategoryProducts").HasKey(x => x.Id);
				builder.HasData(
					new CategoryProduct
					{
						Id = 1,
						Name = "Печать фото",
					});
				builder.Property(x => x.Id).ValueGeneratedOnAdd();
				builder.Property(x => x.Name).IsRequired();
			});

			modelBuilder.Entity<Basket>(builder =>
			{
				builder.ToTable("Baskets").HasKey(x => x.Id);
				builder.HasData(
					new Basket
					{
						Id = 1,
						UserId = 1,
					});
				builder.Property(x => x.Id).ValueGeneratedOnAdd();
				builder.Property(x => x.UserId).IsRequired();
			}); 
			
			modelBuilder.Entity<Order>(builder =>
			{
				builder.Property(x => x.Id).ValueGeneratedOnAdd();
				builder.Property(x => x.BasketId).IsRequired();
			});

		}
	}
}