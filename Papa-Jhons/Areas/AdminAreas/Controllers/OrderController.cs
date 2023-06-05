using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Papa_Jhons.DAL;
using Papa_Jhons.Entities;
using Papa_Jhons.Services;
using Papa_Jhons.Utilities;

namespace Papa_Jhons.Areas.AdminAreas.Controllers
{
    [Area("AdminAreas")]
    [Authorize(Roles = "Admin,Moderator")]

    public class OrderController : Controller
    {
        private readonly PapaJhonsDbContext _context;
        private readonly EmailService _emailService;

        public OrderController(PapaJhonsDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
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
            if (order == null) return Redirect("~/Error/Error");

            return View(order);
        }
        public IActionResult Accept(int id)
        {


            Order order = _context.Orders.Include(x => x.User).FirstOrDefault(o => o.Id == id);
            if (order == null) return Redirect("~/Error/Error");

            order.Status = true;

            _context.SaveChanges();
            string recipientEmail = order.User.Email;
            string subject = "Your order has been accepted";
            string body = "Your order has been accepted. Thank you! The total amount to be paid is " + order.TotalPrice + "₼";


            _emailService.SendEmail(recipientEmail, subject, body);
            return RedirectToAction("Index", "Order");

        }
        public IActionResult Reject(int id)
        {
            Order order = _context.Orders.Include(x => x.User).FirstOrDefault(o => o.Id == id);
            if (order == null) return Redirect("~/Error/Error");

            order.Status = false;

            _context.SaveChanges();
            string recipientEmail = order.User.Email;
            string subject = "Your order has been rejected";
            string body = "Your order has been rejected. Unfortunately, the products you ordered are currently out of stock.Thank you for your understanding.";


            _emailService.SendEmail(recipientEmail, subject, body);
            return RedirectToAction("Index", "Order");

        }
    }
}
