using Microsoft.AspNetCore.Mvc;

namespace Papa_Jhons.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Error()
        {
            return View();
        }
    }
}
