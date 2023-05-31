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

    }
}