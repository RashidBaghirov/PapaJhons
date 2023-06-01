using Microsoft.AspNetCore.Mvc;
using Papa_Jhons.DAL;
using Papa_Jhons.Entities;

namespace Papa_Jhons.Controllers
{
    public class HomeController : Controller
    {
        private readonly PapaJhonsDbContext _context;

        public HomeController(PapaJhonsDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Slider> slider = _context.Sliders.ToList();
            return View(slider);
        }

        public IActionResult Rules()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult Faq()
        {
            return View();
        }


        public IActionResult ContactUs()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ContactUs(string name, string email, string phoneNumber, string comment)
        {
            ContactUs contactUs = new()
            {
                Name = name,
                Email = email,
                PhoneNumber = phoneNumber,
                Comment = comment,
                SendTime = DateTime.Now,
            };

            _context.Contact.Add(contactUs);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

    }
}