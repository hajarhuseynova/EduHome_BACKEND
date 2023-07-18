using EduHome.App.Context;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.areas.Admin.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class TagController : Controller
    {
       
            private readonly EduHomeDbContext _context;
            private readonly IWebHostEnvironment _environment;

            public TagController(EduHomeDbContext context, IWebHostEnvironment environment)
            {
                _context = context;
                _environment = environment;

            }

        public async Task<IActionResult> Index(int page=1)
        {
            int TotalCount = _context.Tag.Where(x => !x.IsDeleted).Count();
            ViewBag.TotalPage = (int)Math.Ceiling((decimal)TotalCount / 5);

            IEnumerable<Tag> Tags = await _context.Tag.
                Where(c => !c.IsDeleted).Skip((page - 1) * 5).Take(5).ToListAsync();
            return View(Tags);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tag Tag)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _context.AddAsync(Tag);
            await _context.SaveChangesAsync();
            return RedirectToAction("index", "Tag");
        }
        [HttpGet]
        
        public async Task<IActionResult> Update(int id)
        {
            Tag? Tag = await _context.Tag.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (Tag == null)
            {
                return NotFound();
            }
            return View(Tag);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Tag postTag)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Tag? Tag = await _context.Tag.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (Tag == null)
            {
                return NotFound();
            }
            Tag.Name = postTag.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction("index", "Tag");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Tag? Tag = await _context.Tag.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (Tag == null)
            {
                return NotFound();
            }

            Tag.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }







    }
}
