using Microsoft.AspNetCore.Mvc;
using Papa_Jhons.DAL;
using Papa_Jhons.Entities;

namespace Papa_Jhons.Controllers
{
    public class OfferController : Controller
    {
        private readonly PapaJhonsDbContext _context;

        public OfferController(PapaJhonsDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Offers> offers = _context.Offers.ToList();
            return View(offers);
        }
    }
}
