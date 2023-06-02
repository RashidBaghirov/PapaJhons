using Microsoft.AspNetCore.Mvc;
using Papa_Jhons.DAL;
using Papa_Jhons.Entities;

namespace Papa_Jhons.Areas.AdminAreas.Controllers
{
    [Area("AdminAreas")]

    public class HomeController : Controller
    {
        private readonly PapaJhonsDbContext _context;

        public HomeController(PapaJhonsDbContext context)
        {
            _context = context;
        }


        public IActionResult Statistics()
        {
            List<OrderItem> orderItems = GetOrderItems();

            return View(orderItems);
        }

        private List<OrderItem> GetOrderItems()
        {
            List<OrderItem> orderItems = _context.OrderItems.ToList();
            return orderItems;
        }
    }
}
