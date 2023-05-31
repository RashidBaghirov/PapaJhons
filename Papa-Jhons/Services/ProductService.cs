using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Papa_Jhons.DAL;
using Papa_Jhons.Entities;

namespace Papa_Jhons.Services
{
    public class ProductService
    {
        private readonly PapaJhonsDbContext _context;


        public ProductService(PapaJhonsDbContext context)
        {
            _context = context;
        }


        public IQueryable<Product> GetAllProducts()
        {
            IQueryable<Product> products = _context.Products.Include(x => x.Category);

            return products;
        }
    }
}
