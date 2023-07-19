using EduHome.App.Context;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class TeacherPositionCatController : Controller
    {
        private readonly EduHomeDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public TeacherPositionCatController(EduHomeDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;

        }
        public async Task<IActionResult> Index(int page=1)
        {
            int TotalCount = _context.TeacherPositionCats.Where(x => !x.IsDeleted).Count();
            ViewBag.TotalPage = (int)Math.Ceiling((decimal)TotalCount / 5);

            IEnumerable<TeacherPositionCat> teacherPositionCats =
                await _context.TeacherPositionCats.Where(c => !c.IsDeleted).Skip((page - 1) * 5).Take(5).ToListAsync();
            return View(teacherPositionCats);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeacherPositionCat teacherPositionCats)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            teacherPositionCats.CreatedDate = DateTime.Now;
            await _context.AddAsync(teacherPositionCats);
            await _context.SaveChangesAsync();
            return RedirectToAction("index", "teacherpositioncat");
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            TeacherPositionCat? teacherPositionCats = await _context.TeacherPositionCats.Where(c => !c.IsDeleted && c.Id == id).FirstOrDefaultAsync();
            if (teacherPositionCats == null)
            {
                return NotFound();
            }
            return View(teacherPositionCats);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, TeacherPositionCat teacherPositionCats)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            TeacherPositionCat? cat = await _context.TeacherPositionCats.Where(c => !c.IsDeleted && c.Id == id).FirstOrDefaultAsync();
            if (cat == null)
            {
                return NotFound();
            }

            cat.UpdatedDate = DateTime.Now;
            cat.Name = teacherPositionCats.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction("index", "teacherpositioncat");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            TeacherPositionCat? category = await _context.TeacherPositionCats.Where(c => !c.IsDeleted && c.Id == id).FirstOrDefaultAsync();
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
