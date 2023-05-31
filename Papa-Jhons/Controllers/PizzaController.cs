using Microsoft.AspNetCore.Mvc;

namespace Papa_Jhons.Controllers
{
    public class PizzaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
