using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Papa_Jhons.DAL;
using Papa_Jhons.Entities;
using Papa_Jhons.Services;
using Papa_Jhons.Utilities;
using Papa_Jhons.ViewModel;

namespace Papa_Jhons.Controllers
{
    public class ProductController : Controller
    {
        private readonly PapaJhonsDbContext _context;
        private readonly ProductService _productService;
        private readonly UserManager<User> _userManager;

        public ProductController(PapaJhonsDbContext context, ProductService productService, UserManager<User> userManager)
        {
            _context = context;
            _productService = productService;
            _userManager = userManager;
        }
        public IActionResult PizzaView(int id)
        {
            List<Product> products = _productService.GetAllProducts().Where(x => x.CategoryId == id).ToList();
            ViewBag.Category = _context.Categories.FirstOrDefault(x => x.Id == id);
            ViewBag.PizzaCategory = _context.PizzaCategory.ToList();
            return View(products);
        }

        [HttpPost]
        public IActionResult PizzaView(int id, int category)
        {
            IQueryable<Product> allproducts = _productService.GetAllProducts();

            if (category != 0)
            {
                switch (category)
                {
                    case 1:
                        allproducts = allproducts.Where(x => x.Category.Name == "Pizza");
                        break;
                    case 2:
                        allproducts = allproducts.Where(x => x.PizzaCategoryId == category);
                        break;
                    case 3:
                        allproducts = allproducts.Where(x => x.PizzaCategoryId == category);
                        break;
                    case 4:
                        allproducts = allproducts.Where(x => x.PizzaCategoryId == category);
                        break;
                    case 5:
                        allproducts = allproducts.Where(x => x.PizzaCategoryId == category);
                        break;
                }
            }
            ViewBag.Category = _context.Categories.FirstOrDefault(x => x.Id == id);
            ViewBag.PizzaCategory = _context.PizzaCategory.ToList();
            List<Product> products = allproducts.ToList();
            return View(products);
        }

        public IActionResult Index(int id)
        {
            List<Product> products = _productService.GetAllProducts().Where(x => x.CategoryId == id).ToList();
            ViewBag.Category = _context.Categories.FirstOrDefault(x => x.Id == id);
            return View(products);
        }




        public async Task<IActionResult> AddBasket(int id, int quantity)
        {

            Product product = _context.Products.FirstOrDefault(p => p.Id == id);

            if (User.Identity.IsAuthenticated)
            {
                User user = await _userManager.FindByNameAsync(User.Identity.Name);
                BasketItem basketItem = _context.BasketItems.FirstOrDefault(b => b.ProductId == product.Id && b.UserId == user.Id);
                if (basketItem == null)
                {
                    basketItem = new BasketItem()
                    {
                        UserId = user.Id,
                        ProductId = product.Id,
                        Count = quantity
                    };
                    _context.BasketItems.Add(basketItem);
                }
                else
                {
                    basketItem.Count++;
                }
                _context.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> RemoveBasketItem(int id)
        {
            Product product = _context.Products.FirstOrDefault(p => p.Id == id);

            if (User.Identity.IsAuthenticated)
            {
                User user = await _userManager.FindByNameAsync(User.Identity.Name);
                BasketItem basketItem = _context.BasketItems.FirstOrDefault(b => b.ProductId == product.Id && b.UserId == user.Id);
                if (basketItem != null)
                {
                    if (basketItem.Count > 1)
                    {
                        basketItem.Count--;
                    }
                    else
                    {
                        _context.BasketItems.Remove(basketItem);
                    }
                    _context.SaveChanges();
                }
            }

            return RedirectToAction("Index", "Home");
        }



    }
}
