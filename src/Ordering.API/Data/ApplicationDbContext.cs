using Microsoft.EntityFrameworkCore;
using Ordering.API.Data.Entities;

namespace Ordering.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<State> States { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSnakeCaseNamingConvention();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<State>(options =>
            {
                options.ToTable("states");

                options.HasKey(x => x.Id);

                options.Property(x => x.Id)
                       .UseIdentityColumn()
                       .ValueGeneratedOnAdd()
                       .HasIdentityOptions(startValue: 10);

                options.HasData(new List<State>()
                {
                    new(){ Id = 1, StateName = "Processing" },
                    new(){ Id = 2, StateName = "Success" },
                    new(){ Id = 3, StateName = "Fail" }
                });
            });

            modelBuilder.Entity<Order>(options =>
            {
                options.ToTable("orders");

                options.HasKey(x => x.Id);

                options.Property(x => x.Id)
                       .UseIdentityColumn()
                       .ValueGeneratedOnAdd()
                       .HasIdentityOptions(startValue: 10);

                options.HasData(new List<Order>()
                {
                    new()
                    { 
                        Id = 1, 
                        OrderDate = new DateTimeOffset(new DateTime(2024,03,01), TimeSpan.FromHours(7)),
                        Note = "Test order",
                        StateId = 1,
                        UserId = 1                         
                    }
                });
            });

            modelBuilder.Entity<OrderDetail>(options =>
            {
                options.ToTable("order_details");

                options.HasKey(x => x.Id);

                options.HasIndex(x => new { x.OrderId, x.ProductId }).IsUnique();

                options.Property(x => x.Id)
                       .UseIdentityColumn()
                       .ValueGeneratedOnAdd()
                       .HasIdentityOptions(startValue: 10);

                options.HasData(new List<OrderDetail>()
                {
                    new()
                    {
                        Id = 1,
                        OrderId = 1,
                        ProductId = 1,
                        ProductName = "iPhone 15 Promax",
                        Price = 1000,
                        Quantity = 1,
                    },
                    new()
                    {
                        Id = 2,
                        OrderId = 1,
                        ProductId = 8,
                        ProductName = "Galaxy Watch",
                        Price = 500,
                        Quantity = 2,
                    }
                });
            });
        }
    }
}
