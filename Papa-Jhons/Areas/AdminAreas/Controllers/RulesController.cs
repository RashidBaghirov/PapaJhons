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
    public class RulesController : Controller
    {
        private readonly PapaJhonsDbContext _context;

        public RulesController(PapaJhonsDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1)
        {
            ViewBag.TotalPage = Math.Ceiling((double)_context.Rules.Count() / 8);
            ViewBag.CurrentPage = page;
            IEnumerable<Rules> rules = _context.Rules.AsNoTracking().Skip((page - 1) * 8).Take(8).AsEnumerable();
            return View(rules);
        }

        [HttpPost]
        public IActionResult Index(string search, int page = 1)
        {
            ViewBag.TotalPage = Math.Ceiling((double)_context.Rules.Count() / 8);
            ViewBag.CurrentPage = page;
            IEnumerable<Rules> rules = _context.Rules.AsNoTracking().Skip((page - 1) * 8).Take(8).AsEnumerable();
            if (!string.IsNullOrEmpty(search))
            {
                rules = rules.Where(x => x.Rule.ToLower().StartsWith(search.ToLower().Substring(0, Math.Min(search.Length, 3)))).ToList();
            }

            return View(rules);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Rules newRules)
        {

            if (!ModelState.IsValid)
            {
                foreach (string message in ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))
                {
                    ModelState.AddModelError("", message);
                }
                return View();
            }
            _context.Rules.Add(newRules);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Edit(int id)
        {
            if (id == 0) return Redirect("~/Error/Error");
            Rules? rules = _context.Rules.FirstOrDefault(c => c.Id == id);
            if (rules is null) return Redirect("~/Error/Error");
            return View(rules);
        }

        [HttpPost]
        public IActionResult Edit(int id, Rules editRules)
        {
            if (id != editRules.Id) return Redirect("~/Error/Error");
            Rules? rules = _context.Rules.FirstOrDefault(c => c.Id == id);
            if (rules is null) return Redirect("~/Error/Error");
            rules.Rule = editRules.Rule;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            if (id == 0) return Redirect("~/Error/Error");
            Rules? rules = _context.Rules.FirstOrDefault(c => c.Id == id);
            return rules is null ? Redirect("~/Error/Error") : View(rules);
        }



        public IActionResult Delete(int id)
        {
            if (id == 0) return Redirect("~/Error/Error");
            Rules? rules = _context.Rules.FirstOrDefault(c => c.Id == id);
            if (rules is null) return Redirect("~/Error/Error");
            return View(rules);
        }

        [HttpPost]
        public IActionResult Delete(int id, Rules deleteRule)
        {
            if (id != deleteRule.Id) return Redirect("~/Error/Error");
            Rules? rule = _context.Rules.FirstOrDefault(c => c.Id == id);
            if (rule is null) return Redirect("~/Error/Error");
            _context.Rules.Remove(rule);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

    }
}
