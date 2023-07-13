using EduHome.App.Context;
using EduHome.App.Extentions;
using EduHome.App.Helpers;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.areas.Admin.Controllers
{
    [Area("Admin")]
    public class CourseController : Controller
    {
        private readonly EduHomeDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public CourseController(EduHomeDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;

        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Course> Courses = await _context.Courses.
                Include(x => x.CourseCategory).
                Where(x => !x.IsDeleted).ToListAsync();
            return View(Courses);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.CourseCategory = await _context.CourseCategories.Where(x => !x.IsDeleted).ToListAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course course)
        {
            ViewBag.CourseCategory = await _context.CourseCategories.Where(x => !x.IsDeleted).ToListAsync();

            if (!ModelState.IsValid)
            {
                return View();
            }
            if (course.FormFile is null)
            {
                ModelState.AddModelError("file", "Image is required");
                return View();
            }


            if (!Helper.isImage(course.FormFile))
            {
                ModelState.AddModelError("file", "Image is required");
                return View();
            }
            if (!Helper.isSizeOk(course.FormFile, 1))
            {
                ModelState.AddModelError("file", "Image size is wrong");
                return View();
            }

            course.CourseCategory = await _context.CourseCategories.Where(x => x.Id == course.CourseCategoryId).FirstOrDefaultAsync();
            course.CreatedDate = DateTime.Now;
            course.Image = course.FormFile.CreateImage(_environment.WebRootPath, "assets/img/");
            await _context.AddAsync(course);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Course");
        }
        public async Task<IActionResult> Update(int id)
        {
            ViewBag.CourseCategory = await _context.CourseCategories.Where(x => !x.IsDeleted).ToListAsync();
            Course? course = await _context.Courses.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (course is null)
            {
                return NotFound();
            }
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Course course)
        {
            ViewBag.CourseCategory = await _context.CourseCategories.Where(x => !x.IsDeleted).ToListAsync();
            Course? UpdateCourse = await _context.Courses.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();

            if (course is null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (course.FormFile is not null)
            {
                if (!Helper.isImage(course.FormFile))
                {
                    ModelState.AddModelError("file", "Image is required");
                    return View();
                }
                if (!Helper.isSizeOk(course.FormFile, 1))
                {
                    ModelState.AddModelError("file", "Image size is wrong");
                    return View();
                }
                UpdateCourse.Image = course.FormFile.CreateImage(_environment.WebRootPath, "assets/img/");
            }


            UpdateCourse.UpdatedDate = DateTime.Now;
            UpdateCourse.Info = course.Info;
            UpdateCourse.Link = course.Link;
            UpdateCourse.CourseCategoryId = course.CourseCategoryId;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {

            Course? course = await _context.Courses.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();

            if (course is null)
            {
                return NotFound();
            }
            course.IsDeleted = true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



    }
}
