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
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();   // создаем базу данных при первом обращении
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
                        PhoneNumber = "111",
                        Password = HashPasswordHelper.HashPassowrd("123456"),
                        Role = Role.Admin
                    });
                builder.Property(x => x.Id).ValueGeneratedOnAdd();
                builder.Property(x => x.PhoneNumber).IsRequired();
                builder.Property(x => x.Password).IsRequired();
                builder.Property(x => x.Role).IsRequired();
            });

        }
    }
}