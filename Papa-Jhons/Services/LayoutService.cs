using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Papa_Jhons.DAL;
using Papa_Jhons.Entities;
using Papa_Jhons.ViewModel;

namespace Papa_Jhons.Services
{
    public class LayoutService
    {
        private readonly PapaJhonsDbContext _context;
        private readonly IHttpContextAccessor _accessor;
        private readonly UserManager<User> _userManager;
        private readonly ProductService _productService;

        public LayoutService(PapaJhonsDbContext context, IHttpContextAccessor accessor, UserManager<User> userManager, ProductService productService)
        {
            _context = context;
            _accessor = accessor;
            _userManager = userManager;
            _productService = productService;
        }

        public async Task<User> GetUserFullName()
        {
            var user = await _userManager.GetUserAsync(_accessor.HttpContext.User);
            return user;
        }

        public List<Category> AllCategory()
        {
            List<Category> categories = _context.Categories.ToList();
            return categories;
        }

        public BasketVm GetBaskets()
        {
            BasketVm basketData = new BasketVm()
            {
                TotalPrice = 0,
                BasketItems = new(),
                Count = 0
            };
            if (_accessor.HttpContext.User.Identity.IsAuthenticated)
            {

                List<BasketItem> basketItems = _context.BasketItems.Include(b => b.User).Where(b => b.User.UserName == _accessor.HttpContext.User.Identity.Name).ToList();
                foreach (BasketItem item in basketItems)
                {
                    Product product = _context.Products.FirstOrDefault(f => f.Id == item.ProductId);
                    if (product != null)
                    {
                        BasketItem basket = new BasketItem()
                        {
                            Product = product,
                            Count = item.Count
                        };
                        basket.Product.Price = product.Price;///
						basketData.BasketItems.Add(basket);
                        basketData.Count++;
                        basketData.TotalPrice += item.Product.Price * item.Count;///
					}
                }
            }

            return basketData;
        }

        public List<Product> GetProducts()
        {
            List<Product> products = _productService.GetAllProducts().ToList();
            return products;
        }


        public List<Order> GetOrderItems()
        {
            List<Order> order = _context.Orders.Include(o => o.OrderItems).Include(o => o.User).ToList();
            return order;
        }


        public List<ContactUs> GetContactUs()
        {
            List<ContactUs> contacts = _context.Contact.ToList();
            return contacts;
        }
    }
}
