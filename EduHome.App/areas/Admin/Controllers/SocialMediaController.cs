using EduHome.App.Context;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.areas.Admin.Controllers
{
        [Area("Admin")]
    public class SocialMediaController : Controller
    {
            private readonly EduHomeDbContext _context;
            private readonly IWebHostEnvironment _environment;

            public SocialMediaController(EduHomeDbContext context, IWebHostEnvironment environment)
            {
                _context = context;
                _environment = environment;

            }

        public async Task<IActionResult> Index(int page=1)
        {
            int TotalCount = _context.SocialMedias.Where(x => !x.IsDeleted).Count();
            ViewBag.TotalPage = (int)Math.Ceiling((decimal)TotalCount / 5);

            IEnumerable<SocialMedia> socialMedias = await _context.SocialMedias.
                Include(x => x.Teacher).
                Where(x => !x.IsDeleted).Skip((page - 1) * 5).Take(5).ToListAsync();

            return View(socialMedias);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Teacher = await _context.Teachers.Where(x => !x.IsDeleted).ToListAsync();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SocialMedia socialMedia)
        {
            ViewBag.Teacher = await _context.Teachers.Where(x => !x.IsDeleted).ToListAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }
            socialMedia.CreatedDate = DateTime.Now;
            await _context.AddAsync(socialMedia);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "SocialMedia");
        }

        public async Task<IActionResult> Update(int id)
        {
            ViewBag.Teacher = await _context.Teachers.Where(x => !x.IsDeleted).ToListAsync();

            SocialMedia? socialMedia = await _context.SocialMedias.
                Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (socialMedia is null)
            {
                return NotFound();
            }
            return View(socialMedia);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, SocialMedia socialMedia)
        {
            ViewBag.Teacher = await _context.Teachers.Where(x => !x.IsDeleted).ToListAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }
            SocialMedia? updatedSocial = await _context.SocialMedias.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (socialMedia is null)
            {
                return NotFound();
            }
            updatedSocial.UpdatedDate = DateTime.Now;
            updatedSocial.TeacherId = socialMedia.TeacherId;
            updatedSocial.Icon = socialMedia.Icon;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            SocialMedia? social = await _context.SocialMedias.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (social is null)
            {
                return NotFound();
            }
            social.IsDeleted = true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
