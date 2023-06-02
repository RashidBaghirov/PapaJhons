﻿using Microsoft.AspNetCore.Mvc;
using Papa_Jhons.DAL;
using Papa_Jhons.Entities;

namespace Papa_Jhons.Areas.AdminAreas.Controllers
{
    [Area("AdminAreas")]
    public class PizzaCategoryController : Controller
    {
        private readonly PapaJhonsDbContext _context;

        public PizzaCategoryController(PapaJhonsDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            IEnumerable<PizzaCategory> category = _context.PizzaCategory.AsEnumerable();
            return View(category);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(PizzaCategory newcategory)
        {

            if (!ModelState.IsValid)
            {
                foreach (string message in ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))
                {
                    ModelState.AddModelError("", message);
                }
                return View();
            }
            bool Isdublicate = _context.PizzaCategory.Any(c => c.Name == newcategory.Name);

            if (Isdublicate)
            {
                ModelState.AddModelError("", "You cannot enter the same data again");
                return View();
            }
            _context.PizzaCategory.Add(newcategory);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            if (id == 0) return NotFound();
            PizzaCategory? category = _context.PizzaCategory.FirstOrDefault(c => c.Id == id);
            if (category is null) return NotFound();
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(int id, PizzaCategory editCategory)
        {
            if (id != editCategory.Id) return NotFound();
            PizzaCategory? category = _context.PizzaCategory.FirstOrDefault(c => c.Id == id);
            if (category is null) return NotFound();
            bool duplicate = _context.PizzaCategory.Any(c => c.Name == editCategory.Name && category.Name != editCategory.Name);
            if (duplicate)
            {
                ModelState.AddModelError("Name", "This  category name is now available");
                return View();
            }
            category.Name = editCategory.Name;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            if (id == 0) return NotFound();
            PizzaCategory? category = _context.PizzaCategory.FirstOrDefault(c => c.Id == id);
            return category is null ? BadRequest() : View(category);
        }



        public IActionResult Delete(int id)
        {
            if (id == 0) return NotFound();
            PizzaCategory? category = _context.PizzaCategory.FirstOrDefault(c => c.Id == id);
            if (category is null) return NotFound();
            return View(category);
        }

        [HttpPost]
        public IActionResult Delete(int id, PizzaCategory deleteCategory)
        {
            if (id != deleteCategory.Id) return NotFound();
            PizzaCategory? category = _context.PizzaCategory.FirstOrDefault(c => c.Id == id);
            if (category is null) return NotFound();
            _context.PizzaCategory.Remove(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}