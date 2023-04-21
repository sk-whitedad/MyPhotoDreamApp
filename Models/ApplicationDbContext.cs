using Microsoft.EntityFrameworkCore;
using MyPhotoDreamApp.Enum;
using MyPhotoDreamApp.Helpers;

namespace MyPhotoDreamApp.Models
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
            });

        }
    }
}