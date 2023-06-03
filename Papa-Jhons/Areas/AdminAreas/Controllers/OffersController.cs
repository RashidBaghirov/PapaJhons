﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papa_Jhons.DAL;
using Papa_Jhons.Entities;
using Papa_Jhons.Utilities;
using Papa_Jhons.Utilities.Extension;

namespace Papa_Jhons.Areas.AdminAreas.Controllers
{
    [Area("AdminAreas")]
    [Authorize(Roles = "Admin,Moderator")]

    public class OffersController : Controller
    {
        private readonly PapaJhonsDbContext _context;
        private readonly IWebHostEnvironment _env;

        public OffersController(PapaJhonsDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            IEnumerable<Offers> offers = _context.Offers.AsEnumerable();
            return View(offers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Offers createOffers)
        {
            if (createOffers.Image is null)
            {
                ModelState.AddModelError("image", "Choose image");
                return View();
            }
            if (!createOffers.Image.IsValidFile("image/"))
            {
                ModelState.AddModelError("Image", "Choose image type file");
                return View();
            }
            if (!createOffers.Image.IsValidLength(1))
            {
                ModelState.AddModelError("Image", "Max Size 1MB");
                return View();
            }
            string imagesFolderPath = Path.Combine("assets", "images", "Offers");

            createOffers.ImagePath = await createOffers.Image.CreateImage(_env.WebRootPath, imagesFolderPath);

            _context.Offers.Add(createOffers);

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            if (id == 0) return Redirect("~/Error/Error");
            Offers? offers = _context.Offers.FirstOrDefault(s => s.Id == id);
            if (offers is null) return Redirect("~/Error/Error");

            return View(offers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Offers editedoffers)
        {
            if (id == 0) return Redirect("~/Error/Error");
            Offers? offers = _context.Offers.FirstOrDefault(s => s.Id == id);
            if (!ModelState.IsValid)
            {
                return View();
            }

            _context.Entry(offers).CurrentValues.SetValues(editedoffers);

            if (editedoffers.Image is not null)
            {
                string imagesFolderPath = Path.Combine("assets", "images", "Offers");

                string FullPath = Path.Combine(_env.WebRootPath, imagesFolderPath, offers.ImagePath);

                ExtensionMethods.DeleteImage(FullPath);

                offers.ImagePath = await editedoffers.Image.CreateImage(_env.WebRootPath, imagesFolderPath);

            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Detail(int id)
        {
            if (id == 0) return Redirect("~/Error/Error");

            Offers? offers = _context.Offers.FirstOrDefault(s => s.Id == id);

            if (offers is null)
            {
                return Redirect("~/Error/Error");
            }

            return View(offers);
        }

        public async Task<IActionResult> Delete(int id, Offers deleted)
        {
            if (id == 0) return Redirect("~/Error/Error");

            Offers? offers = _context.Offers.FirstOrDefault(s => s.Id == id);
            if (offers is null)
            {
                return Redirect("~/Error/Error");
            }

            if (deleted.Image is null)
            {
                string imagesFolderPath = Path.Combine("assets", "images", "Offers");

                string FullPath = Path.Combine(_env.WebRootPath, imagesFolderPath, offers.ImagePath);

                ExtensionMethods.DeleteImage(FullPath);
            }

            _context.Offers.Remove(offers);

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
