using EduHome.App.Context;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.areas.Admin.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class PositionCategoryController : Controller
    {
        private readonly EduHomeDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public PositionCategoryController(EduHomeDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;

        }
        public async Task<IActionResult> Index(int page=1)
        {
            int TotalCount = _context.PositionCategories.Where(x => !x.IsDeleted).Count();
            ViewBag.TotalPage = (int)Math.Ceiling((decimal)TotalCount / 5);

            IEnumerable<PositionCategory> PositionCategories =
                await _context.PositionCategories.Where(c => !c.IsDeleted).Skip((page - 1) * 5).Take(5).ToListAsync();
            return View(PositionCategories);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PositionCategory positionCategory)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            positionCategory.CreatedDate = DateTime.Now;
            await _context.AddAsync(positionCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction("index", "positioncategory");
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            PositionCategory? positionCategory = await _context.PositionCategories.Where(c => !c.IsDeleted && c.Id == id).FirstOrDefaultAsync();
            if (positionCategory == null)
            {
                return NotFound();
            }
            return View(positionCategory);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, PositionCategory positionCategory)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            PositionCategory? cat = await _context.PositionCategories.Where(c => !c.IsDeleted && c.Id == id).FirstOrDefaultAsync();
            if (cat == null)
            {
                return NotFound();
            }

            cat.UpdatedDate = DateTime.Now;
            cat.Name = positionCategory.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction("index", "positioncategory");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            PositionCategory? category = await _context.PositionCategories.Where(c => !c.IsDeleted && c.Id == id).FirstOrDefaultAsync();
            if (category == null)
            {
                return NotFound();
            }
            category.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
