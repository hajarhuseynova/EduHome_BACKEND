using EduHome.App.Context;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.areas.Admin.Controllers
{
   
    [Area("Manage")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class CourseCategoryController : Controller
    {
        private readonly EduHomeDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public CourseCategoryController(EduHomeDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;

        }
        public async Task<IActionResult> Index(int page=1)
        {
            int TotalCount = _context.CourseCategories.Where(x => !x.IsDeleted).Count();
            ViewBag.TotalPage = (int)Math.Ceiling((decimal)TotalCount / 5);
            IEnumerable<CourseCategory> CourseCategories =
                await _context.CourseCategories.Where(c => !c.IsDeleted).Skip((page - 1) * 5).Take(5).ToListAsync();
            return View(CourseCategories);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseCategory courseCategory)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            courseCategory.CreatedDate = DateTime.Now;
            await _context.AddAsync(courseCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction("index", "coursecategory");
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            CourseCategory? courseCategory = await _context.CourseCategories.Where(c => !c.IsDeleted && c.Id == id).FirstOrDefaultAsync();
            if (courseCategory == null)
            {
                return NotFound();
            }
            return View(courseCategory);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, CourseCategory courseCategory)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            CourseCategory? cat = await _context.CourseCategories.Where(c => !c.IsDeleted && c.Id == id).FirstOrDefaultAsync();
            if (cat == null)
            {
                return NotFound();
            }

            cat.UpdatedDate = DateTime.Now;
            cat.Name = courseCategory.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction("index", "coursecategory");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            CourseCategory? category = await _context.CourseCategories.Where(c => !c.IsDeleted && c.Id == id).FirstOrDefaultAsync();
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
