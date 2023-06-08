using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Papa_Jhons.DAL;
using Papa_Jhons.Entities;
using Papa_Jhons.Utilities;

namespace Papa_Jhons.Areas.AdminAreas.Controllers
{
    [Area("AdminAreas")]
    [Authorize(Roles = "Admin,Moderator")]
    public class AboutController : Controller
    {
        private readonly PapaJhonsDbContext _context;

        public AboutController(PapaJhonsDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1)
        {
            ViewBag.TotalPage = Math.Ceiling((double)_context.Abouts.Count() / 8);
            ViewBag.CurrentPage = page;
            IEnumerable<About> abouts = _context.Abouts.AsNoTracking().Skip((page - 1) * 8).Take(8).AsEnumerable();
            return View(abouts);
        }

        [HttpPost]
        public IActionResult Index(string search, int page = 1)
        {
            ViewBag.TotalPage = Math.Ceiling((double)_context.Abouts.Count() / 8);
            ViewBag.CurrentPage = page;
            IEnumerable<About> abouts = _context.Abouts.AsNoTracking().Skip((page - 1) * 8).Take(8).AsEnumerable();
            if (!string.IsNullOrEmpty(search))
            {
                abouts = abouts.Where(x => x.Abouts.ToLower().StartsWith(search.ToLower().Substring(0, Math.Min(search.Length, 3)))).ToList();
            }

            return View(abouts);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(About newAbouts)
        {

            if (!ModelState.IsValid)
            {
                foreach (string message in ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))
                {
                    ModelState.AddModelError("", message);
                }
                return View();
            }
            _context.Abouts.Add(newAbouts);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Edit(int id)
        {
            if (id == 0) return Redirect("~/Error/Error");
            About? about = _context.Abouts.FirstOrDefault(c => c.Id == id);
            if (about is null) return Redirect("~/Error/Error");
            return View(about);
        }

        [HttpPost]
        public IActionResult Edit(int id, About editAbout)
        {
            if (id != editAbout.Id) return Redirect("~/Error/Error");
            About? abouts = _context.Abouts.FirstOrDefault(c => c.Id == id);
            if (abouts is null) return Redirect("~/Error/Error");
            abouts.Abouts = editAbout.Abouts;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            if (id == 0) return Redirect("~/Error/Error");
            About? about = _context.Abouts.FirstOrDefault(c => c.Id == id);
            return about is null ? Redirect("~/Error/Error") : View(about);
        }



        public IActionResult Delete(int id)
        {
            if (id == 0) return Redirect("~/Error/Error");
            About? abouts = _context.Abouts.FirstOrDefault(c => c.Id == id);
            if (abouts is null) return Redirect("~/Error/Error");
            return View(abouts);
        }

        [HttpPost]
        public IActionResult Delete(int id, About deleteAbout)
        {
            if (id != deleteAbout.Id) return Redirect("~/Error/Error");
            About? abouts = _context.Abouts.FirstOrDefault(c => c.Id == id);
            if (abouts is null) return Redirect("~/Error/Error");
            _context.Abouts.Remove(abouts);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

    }
}
