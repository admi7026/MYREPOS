using Catalog.API.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSnakeCaseNamingConvention();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(options =>
            {
                options.ToTable("categories");

                options.HasKey(x => x.Id);

                options.Property(x => x.Id)
                       .UseIdentityColumn()
                       .ValueGeneratedOnAdd()
                       .HasIdentityOptions(startValue: 10);

                options.HasData(new List<Category>()
                {
                    new(){ Id = 1, CategoryName = "Apple" },
                    new(){ Id = 2, CategoryName = "Samsung" }
                });
            });

            modelBuilder.Entity<Product>(options =>
            {
                options.ToTable("products");

                options.HasKey(x => x.Id);

                options.Property(x => x.Id)
                       .UseIdentityColumn()
                       .ValueGeneratedOnAdd()
                       .HasIdentityOptions(startValue: 11);

                options.HasData(new List<Product>()
                {
                    new(){ Id = 1, ProductName = "iPhone 15 Promax", Price = 1000, UnitInStock = 10, CategoryId = 1 },
                    new(){ Id = 2, ProductName = "iPhone 15 Gold", Price = 500, UnitInStock = 10, CategoryId = 1 },
                    new(){ Id = 3, ProductName = "Apple Watch Ultra", Price = 1000, UnitInStock = 10, CategoryId = 1 },
                    new(){ Id = 4, ProductName = "Apple Airpod 3", Price = 300, UnitInStock = 10, CategoryId = 1 },
                    new(){ Id = 5, ProductName = "Vision Pro", Price = 3000, UnitInStock = 10, CategoryId = 1 },
                    new(){ Id = 6, ProductName = "Samsung S24", Price = 800, UnitInStock = 10, CategoryId = 2 },
                    new(){ Id = 7, ProductName = "Samsung A50", Price = 900, UnitInStock = 10, CategoryId = 2 },
                    new(){ Id = 8, ProductName = "Galaxy Watch", Price = 500, UnitInStock = 10, CategoryId = 2 },
                    new(){ Id = 9, ProductName = "Samsung S23", Price = 700, UnitInStock = 10, CategoryId = 2 },
                    new(){ Id = 10, ProductName = "Samsung A35", Price = 100, UnitInStock = 10, CategoryId = 2 }
                });
            });
        }
    }
}
