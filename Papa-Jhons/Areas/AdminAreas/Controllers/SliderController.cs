using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Papa_Jhons.DAL;
using Papa_Jhons.Entities;
using Papa_Jhons.Utilities;
using Papa_Jhons.Utilities.Extension;
using System.Net.Security;

namespace Papa_Jhons.Areas.AdminAreas.Controllers
{
    [Area("AdminAreas")]
    [Authorize(Roles = "Admin,Moderator")]

    public class SliderController : Controller
    {
        readonly PapaJhonsDbContext _context;
        readonly IWebHostEnvironment _env;

        public SliderController(PapaJhonsDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            IEnumerable<Slider> sliders = _context.Sliders.AsEnumerable();
            return View(sliders);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Slider newSlider)
        {
            if (newSlider.Image is null)
            {
                ModelState.AddModelError("Image", "Please Select Image");
                return View();
            }

            if (!newSlider.Image.IsValidFile("image/"))
            {
                ModelState.AddModelError("Image", "Please Select Image Tag");
                return View();
            }
            if (!newSlider.Image.IsValidLength(2))
            {
                ModelState.AddModelError("Image", "Please Select Image which size max 2MB");
                return View();
            }
            if (!Imports(newSlider))
            {
                return View();
            }
            var maxOrder = await _context.Sliders.OrderByDescending(s => s.Order).Select(s => s.Order).FirstOrDefaultAsync();
            var imagefolderPath = Path.Combine(_env.WebRootPath, "assets", "images", "skins", "fashion");

            newSlider.ImagePath = await newSlider.Image.CreateImage(imagefolderPath, "slider");
            newSlider.Order = (byte)(maxOrder + 1);
            _context.Sliders.Add(newSlider);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            if (id <= 0)
            {
                return Redirect("~/Error/Error");
            }
            Slider slider = _context.Sliders.FirstOrDefault(s => s.Id == id);
            if (slider is null)
            {
                return Redirect("~/Error/Error");
            }
            return View(slider);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Slider edited)
        {
            if (id != edited.Id) return Redirect("~/Error/Error");
            Slider slider = _context.Sliders.FirstOrDefault(s => s.Id == id);
            if (slider is null)
            {
                return Redirect("~/Error/Error");
            }
            if (!ModelState.IsValid) return View(slider);
            _context.Entry<Slider>(slider).CurrentValues.SetValues(edited);

            if (edited.Image is not null)
            {
                var imagefolderPath = Path.Combine(_env.WebRootPath, "assets", "images", "skins", "fashion");
                string filepath = Path.Combine(imagefolderPath, "slider", edited.ImagePath);
                ExtensionMethods.DeleteImage(filepath);
                slider.ImagePath = await edited.Image.CreateImage(imagefolderPath, "slider");
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            if (id == 0) return Redirect("~/Error/Error");
            Slider? slider = _context.Sliders.FirstOrDefault(s => s.Id == id);
            return slider is null ? Redirect("~/Error/Error") : View(slider);
        }

        public IActionResult Delete(int id)
        {
            if (id == 0) return Redirect("~/Error/Error");
            Slider? slider = _context.Sliders.FirstOrDefault(s => s.Id == id);
            if (slider is null) return Redirect("~/Error/Error");
            return View(slider);
        }

        [HttpPost]
        public IActionResult Delete(int id, Slider deleteslider)
        {
            if (id != deleteslider.Id) return Redirect("~/Error/Error");
            Slider? slider = _context.Sliders.FirstOrDefault(s => s.Id == id);
            if (slider is null) return Redirect("~/Error/Error");
            var imagefolderPath = Path.Combine(_env.WebRootPath, "assets", "images", "skins", "fashion");

            string filepath = Path.Combine(imagefolderPath, "slider", slider.ImagePath);
            ExtensionMethods.DeleteImage(filepath);
            _context.Sliders.Remove(slider);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        bool Imports(Slider newSlider)
        {
            if (newSlider.Order is null)
            {
                ModelState.AddModelError("", "Note the Order!");
                return false;
            }

            return true;
        }

    }
}
