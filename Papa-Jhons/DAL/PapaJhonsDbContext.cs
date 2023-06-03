using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Papa_Jhons.Entities;

namespace Papa_Jhons.DAL
{
    public class PapaJhonsDbContext : IdentityDbContext<User>
    {
        public PapaJhonsDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Offers> Offers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<PizzaCategory> PizzaCategory { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ContactUs> Contact { get; set; }
        public DbSet<Rules> Rules { get; set; }






    }
}
