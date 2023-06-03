using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Papa_Jhons.Areas.AdminAreas.ViewModel;
using Papa_Jhons.DAL;
using Papa_Jhons.Entities;
using Papa_Jhons.Services;
using Papa_Jhons.Utilities;

namespace Papa_Jhons.Areas.AdminAreas.Controllers
{
    [Area("AdminAreas")]
    [Authorize(Roles = "Admin,Moderator")]

    public class HomeController : Controller
    {
        private readonly PapaJhonsDbContext _context;
        private readonly ProductService _productService;

        public HomeController(PapaJhonsDbContext context, ProductService productService)
        {
            _context = context;
            _productService = productService;
        }


        public IActionResult Statistics()
        {
            List<Product> products = _productService.GetAllProducts().ToList();
            List<Category> categories = _context.Categories.Include(x => x.Products).ToList();
            List<CategoryChartVm> categoryProductNames = categories.Select(cat => new CategoryChartVm
            {
                CategoryName = cat.Name,
                ProductCount = products.Count(p => p.CategoryId == cat.Id),
            }).ToList();
            var categoryLabels = categories.Select(cat => cat.Name).ToList();
            var categoryData = categoryProductNames.Select(item => item.ProductCount).ToList();
            ViewBag.CategoryLabels = categoryLabels;
            ViewBag.CategoryData = categoryData;

            List<User> users = _context.Users.Include(x => x.Orders).ToList();
            List<UserOrderStatisticVm> userStatistics = users.Select(u => new UserOrderStatisticVm
            {
                UserId = u.Id,
                UserName = u.FullName,
                OrderCount = u.Orders.Count
            })
            .OrderByDescending(stat => stat.OrderCount)
            .ToList();

            var userLabels = userStatistics.Select(stat => stat.UserName).ToList();
            var userData = userStatistics.Select(stat => stat.OrderCount).ToList();

            ViewBag.UserLabels = userLabels;
            ViewBag.UserData = userData;

            return View();
        }



    }
}
