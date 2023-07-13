using EduHome.App.Context;
using EduHome.App.Extentions;
using EduHome.App.Helpers;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.areas.Admin.Controllers
{
    [Area("Admin")]
    public class AboutController : Controller
    {
        private readonly EduHomeDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public AboutController(EduHomeDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;

        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<About> abouts = await _context.Abouts.Where(x => !x.IsDeleted).ToListAsync();
            return View(abouts);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(About about)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (about.FormFile == null)
            {
                ModelState.AddModelError("FormFile", "Wrong!");
                return View();
            }


            if (!Helper.isImage(about.FormFile))
            {
                ModelState.AddModelError("FormFile", "Wronggg!");
                return View();
            }
            if (!Helper.isSizeOk(about.FormFile, 1))
            {
                ModelState.AddModelError("FormFile", "Wronggg!");
                return View();
            }

            about.Image = about.FormFile.CreateImage(_environment.WebRootPath, "assets/img/");
            about.CreatedDate = DateTime.Now;
            await _context.AddAsync(about);

            await _context.SaveChangesAsync();
            return RedirectToAction("index", "about");

        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            About? about = await _context.Abouts.
                  Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (about == null)
            {
                return NotFound();
            }
            return View(about);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, About about)
        {
            About? UpdateAbout= await _context.Abouts.
                Where(c => !c.IsDeleted && c.Id == id).FirstOrDefaultAsync();

            if (about == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(UpdateAbout);
            }


            if (about.FormFile != null)
            {
                if (!Helper.isImage(about.FormFile))
                {
                    ModelState.AddModelError("FormFile", "Wronggg!");
                    return View();
                }
                if (!Helper.isSizeOk(about.FormFile, 1))
                {
                    ModelState.AddModelError("FormFile", "Wronggg!");
                    return View();
                }
                Helper.RemoveImage(_environment.WebRootPath, "assets/images", UpdateAbout.Image);
                UpdateAbout.Image = about.FormFile.CreateImage(_environment.WebRootPath, "assets/images");
            }


            UpdateAbout.Title = about.Title;
            UpdateAbout.Description = about.Description;
            UpdateAbout.DescriptionResponsive = about.DescriptionResponsive;
            UpdateAbout.Link= about.Link;
            UpdateAbout.UpdatedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Delete(int id)
        {
            About? about = await _context.Abouts.
                  Where(c => !c.IsDeleted && c.Id == id).FirstOrDefaultAsync();
            if (about == null)
            {
                return NotFound();
            }
            about.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



    }
}
