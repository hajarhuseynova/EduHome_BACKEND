using EduHome.App.Context;
using EduHome.App.Extentions;
using EduHome.App.Helpers;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeacherController : Controller
    {
        private readonly EduHomeDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public TeacherController(EduHomeDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;

        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Teacher> teachers =
             await _context.Teachers
             .Include(t => t.SocialMedias).Include(t => t.teacherPositionCat).
             Where(t => !t.IsDeleted).ToListAsync();
            return View(teachers);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.TeacherPositionCats = await _context.TeacherPositionCats.Where(x => !x.IsDeleted).ToListAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(Teacher teacher)
        {
            ViewBag.TeacherPositionCats = await _context.TeacherPositionCats.Where(x => !x.IsDeleted).ToListAsync();

            if (!ModelState.IsValid)
            {
                return View();
            }
            if (teacher.File is null)
            {
                ModelState.AddModelError("file", "Image is required!");
                return View();
            }
            if (!Helper.isImage(teacher.File))
            {
                ModelState.AddModelError("file", "Image is required!");
                return View();
            }
            if (!Helper.isSizeOk(teacher.File, 1))
            {
                ModelState.AddModelError("file", "Image size is wrong!");
                return View();
            }

            teacher.CreatedDate = DateTime.Now;
            teacher.teacherPositionCat = _context.TeacherPositionCats.Where(x => x.Id == teacher.TeacherPositionCatId).FirstOrDefault();
            teacher.Image = teacher.File.CreateImage(_environment.WebRootPath, "assets/img/");
            await _context.AddAsync(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Teacher");
        }

        public async Task<IActionResult> Update(int id)
        {
            Teacher? teacher = await _context.Teachers.
                Where(t => !t.IsDeleted && t.Id == id).FirstOrDefaultAsync();


            ViewBag.TeacherPositionCats = await _context.TeacherPositionCats.Where(x => !x.IsDeleted).ToListAsync();

            if (teacher is null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Teacher teacher)
        {
            Teacher? updatedteacher = await _context.
                Teachers.Where(t => !t.IsDeleted && t.Id == id).FirstOrDefaultAsync();


            ViewBag.TeacherPositionCats = await _context.TeacherPositionCats.Where(x => !x.IsDeleted).ToListAsync();

            if (teacher is null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(updatedteacher);
            }

            if (teacher.File != null)
            {
                if (!Helper.isImage(teacher.File))
                {
                    ModelState.AddModelError("FormFile", "Wronggg!");
                    return View();
                }
                if (!Helper.isSizeOk(teacher.File, 1))
                {
                    ModelState.AddModelError("FormFile", "Wronggg!");
                    return View();
                }

                updatedteacher.Image = teacher.File.CreateImage(_environment.WebRootPath, "assets/img/team");
            }

            updatedteacher.UpdatedDate = DateTime.Now;
            updatedteacher.FullName = teacher.FullName;
            updatedteacher.TeacherPositionCatId = teacher.TeacherPositionCatId;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Teacher? teacher = await _context.Teachers.Where(t => !t.IsDeleted && t.Id == id).FirstOrDefaultAsync();
            if (teacher is null)
            {
                return NotFound();
            }
            teacher.IsDeleted = true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
