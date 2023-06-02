using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Papa_Jhons.DAL;
using Papa_Jhons.Entities;

namespace Papa_Jhons.Areas.AdminAreas.Controllers
{
    [Area("AdminAreas")]
    public class OrderController : Controller
    {
        private readonly PapaJhonsDbContext _context;

        public OrderController(PapaJhonsDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Order> orders = _context.Orders.ToList();
            return View(orders);
        }
        public IActionResult Edit(int id)
        {
            Order order = _context.Orders.Include(o => o.OrderItems).Include(o => o.User).FirstOrDefault(o => o.Id == id);
            ViewBag.Products = _context.Products.ToList();
            if (order == null) return NotFound();

            return View(order);
        }
        public IActionResult Accept(int id)
        {


            Order order = _context.Orders.FirstOrDefault(o => o.Id == id);
            if (order == null) return NotFound();

            order.Status = true;

            _context.SaveChanges();

            return RedirectToAction("Index", "Order");

        }
        public IActionResult Reject(int id)
        {


            Order order = _context.Orders.FirstOrDefault(o => o.Id == id);
            if (order == null) return NotFound();

            order.Status = false;

            _context.SaveChanges();

            return RedirectToAction("Index", "Order");

        }
    }
}
