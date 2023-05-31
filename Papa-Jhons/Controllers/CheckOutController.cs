using Microsoft.AspNetCore.Mvc;
using Papa_Jhons.DAL;
using Papa_Jhons.Services;

namespace Papa_Jhons.Controllers
{
    public class CheckOutController : Controller
    {
        private readonly PapaJhonsDbContext _context;
        private readonly ProductService _productService;

        public CheckOutController(PapaJhonsDbContext context, ProductService productService)
        {
            _context = context;
            _productService = productService;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
