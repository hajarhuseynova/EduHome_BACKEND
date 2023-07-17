using EduHome.App.Context;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.areas.Admin.Controllers
{
    [Area("Admin")]
    public class FacultyController : Controller
    {
        private readonly EduHomeDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public FacultyController(EduHomeDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;

        }
        public async Task<IActionResult> Index(int page=1)
        {
            int TotalCount = _context.Courses.Where(x => !x.IsDeleted).Count();
            ViewBag.TotalPage = (int)Math.Ceiling((decimal)TotalCount / 5);

            IEnumerable<Faculty> Faculties =
                await _context.Faculty.Where(c => !c.IsDeleted).Skip((page - 1) * 5).Take(5).ToListAsync();
            return View(Faculties);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Faculty faculty)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            faculty.CreatedDate = DateTime.Now;
            await _context.AddAsync(faculty);
            await _context.SaveChangesAsync();
            return RedirectToAction("index", "faculty");
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Faculty? faculty = await _context.Faculty.Where(c => !c.IsDeleted && c.Id == id).FirstOrDefaultAsync();
            if (faculty == null)
            {
                return NotFound();
            }
            return View(faculty);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Faculty faculty)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Faculty? fac = await _context.Faculty.Where(c => !c.IsDeleted && c.Id == id).FirstOrDefaultAsync();
            if (fac == null)
            {
                return NotFound();
            }

            fac.UpdatedDate = DateTime.Now;
            fac.Name = faculty.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction("index", "faculty");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Faculty? faculty = await _context.Faculty.Where(c => !c.IsDeleted && c.Id == id).FirstOrDefaultAsync();
            if (faculty == null)
            {
                return NotFound();
            }
            faculty.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
