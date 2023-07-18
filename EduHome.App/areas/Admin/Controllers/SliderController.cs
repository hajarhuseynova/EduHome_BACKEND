using EduHome.App.Context;
using EduHome.App.Extentions;
using EduHome.App.Helpers;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.areas.Admin.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class SliderController : Controller
    {
        private readonly EduHomeDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public SliderController(EduHomeDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;

        }

        public async Task<IActionResult> Index(int page=1)
        {
            int TotalCount = _context.Slides.Where(x => !x.IsDeleted).Count();
            ViewBag.TotalPage = (int)Math.Ceiling((decimal)TotalCount / 5);

            IEnumerable<Slider> sliders = await _context.Slides.Where(x => !x.IsDeleted).Skip((page - 1) * 5).Take(5).ToListAsync();
            return View(sliders);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Slider slider)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (slider.FormFile == null)
            {
                ModelState.AddModelError("FormFile", "Wrong!");
                return View();
            }


            if (!Helper.isImage(slider.FormFile))
            {
                ModelState.AddModelError("FormFile", "Wronggg!");
                return View();
            }
            if (!Helper.isSizeOk(slider.FormFile, 1))
            {
                ModelState.AddModelError("FormFile", "Wronggg!");
                return View();
            }

            slider.Image = slider.FormFile.CreateImage(_environment.WebRootPath, "assets/img/");
            slider.CreatedDate = DateTime.Now;
            await _context.AddAsync(slider);

            await _context.SaveChangesAsync();
            return RedirectToAction("index", "slider");

        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Slider? slider = await _context.Slides.
                  Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (slider == null)
            {
                return NotFound();
            }
            return View(slider);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Slider slide)
        {
            Slider? UpdateSlider= await _context.Slides.
                Where(c => !c.IsDeleted && c.Id == id).FirstOrDefaultAsync();

            if (slide == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(UpdateSlider);
            }


            if (slide.FormFile != null)
            {
                if (!Helper.isImage(slide.FormFile))
                {
                    ModelState.AddModelError("FormFile", "Wronggg!");
                    return View();
                }
                if (!Helper.isSizeOk(slide.FormFile, 1))
                {
                    ModelState.AddModelError("FormFile", "Wronggg!");
                    return View();
                }
                Helper.RemoveImage(_environment.WebRootPath, "assets/images", UpdateSlider.Image);
                UpdateSlider.Image = slide.FormFile.CreateImage(_environment.WebRootPath, "assets/images");
            }


            UpdateSlider.Title = slide.Title;
            UpdateSlider.Description = slide.Description;
            UpdateSlider.Link= slide.Link;
            UpdateSlider.UpdatedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Delete(int id)
        {
            Slider? slider = await _context.Slides.
                  Where(c => !c.IsDeleted && c.Id == id).FirstOrDefaultAsync();
            if (slider == null)
            {
                return NotFound();
            }
            slider.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



    }
}
