using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Papa_Jhons.DAL;
using Papa_Jhons.Entities;
using Papa_Jhons.ViewModel;

namespace Papa_Jhons.Controllers
{
    public class OrderController : Controller
    {
        private readonly PapaJhonsDbContext _context;
        private readonly UserManager<User> _userManager;

        public OrderController(PapaJhonsDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Checkout()
        {

            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            OrderVm model = new()
            {
                Fullname = user.FullName,
                Username = user.UserName,
                Email = user.Email,
                BasketItems = _context.BasketItems.Include(p => p.Product).Where(c => c.UserId == user.Id).ToList()


            };
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(OrderVm orderVM)
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            OrderVm model = new()
            {
                Fullname = orderVM.Fullname,
                Username = orderVM.Username,
                Email = orderVM.Email,
                Message = orderVM.Message,
                Number = orderVM.Number,
                Address = orderVM.Address,
                BasketItems = _context.BasketItems.Include(p => p.Product).Where(c => c.UserId == user.Id).ToList()


            };
            if (!ModelState.IsValid) return View(model);
            if (model.BasketItems.Count == 0) return RedirectToAction("Index", "Home");


            Order order = new Order()
            {
                City = orderVM.City,
                Address = orderVM.Address,
                TotalPrice = 0,
                Date = DateTime.Now,
                UserId = user.Id,
                Number = orderVM.Number,
                Message = orderVM.Message,
                Status = false
            };


            foreach (BasketItem item in model.BasketItems)
            {

                OrderItem orderItem = new OrderItem
                {
                    Name = item.Product.Name,
                    Price = (double)item.Product.Price,
                    ProductId = item.ProductId,
                    Quantity = item.Count,
                    Order = order
                };
                order.TotalPrice += (double)item.Product.Price * item.Count;
                _context.OrderItems.Add(orderItem);
            }
            _context.BasketItems.RemoveRange(model.BasketItems);
            _context.Orders.Add(order);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}
