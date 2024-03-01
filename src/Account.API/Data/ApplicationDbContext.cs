using Account.API.Data.Entities;
using Account.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Account.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSnakeCaseNamingConvention();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Role>(options =>
            {
                options.ToTable("roles");

                options.HasKey(x => x.Id);

                options.Property(x => x.Id)
                       .UseIdentityColumn()
                       .ValueGeneratedOnAdd()
                       .HasIdentityOptions(startValue: 10);

                options.HasData(new List<Role>()
                { 
                    new()
                    {
                        Id = 1,
                        RoleName = "Administrator"
                    },
                    new()
                    {
                        Id = 2,
                        RoleName = "User"
                    }
                });
            });

            modelBuilder.Entity<User>(options =>
            {
                options.ToTable("users");

                options.HasKey(x => x.Id);

                options.Property(x => x.Id)
                       .UseIdentityColumn()
                       .ValueGeneratedOnAdd()
                       .HasIdentityOptions(startValue: 10);

                var password = "123456";

                options.HasData(new List<User>()
                {
                    new User()
                    {
                        Id = 1,
                        UserName = "admin@gmail.com",
                        Password = Md5Helper.GetMd5Hash(password),
                        RoleId = 1
                    }
                });
            });
        }
    }
}
