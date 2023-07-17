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
        public async Task<IActionResult> Index(int page = 1)
        {
            int TotalCount = _context.Courses.Where(x => !x.IsDeleted).Count();
            ViewBag.TotalPage = (int)Math.Ceiling((decimal)TotalCount / 5);
        
            IEnumerable<Course> Courses = await _context.Courses.
                Include(x => x.CourseCategory).
                Include(x=>x.Feature).
                Where(x => !x.IsDeleted).Skip((page - 1) * 5).Take(5).ToListAsync();
            return View(Courses);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.CourseCategory = await _context.CourseCategories.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Tag = await _context.Tag.Where(x => !x.IsDeleted).ToListAsync();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course course)
        {
            ViewBag.CourseCategory = await _context.CourseCategories.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Tag = await _context.Tag.Where(x => !x.IsDeleted).ToListAsync();


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

            foreach (var item in course.TagIds)
            {
                if (!await _context.Tag.AnyAsync(x => x.Id == item))
                {
                    ModelState.AddModelError("", "Wrongg!");
                    return View(course);
                }
                CourseTag courseTag = new CourseTag
                {
                    TagId = item,
                    Course = course,
                    CreatedDate = DateTime.Now
                };
                await _context.CourseTags.AddAsync(courseTag);
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
            ViewBag.Tag = await _context.Tag.Where(x => !x.IsDeleted).ToListAsync();



            Course? course = await _context.Courses.
                Include(x=>x.Feature).AsNoTrackingWithIdentityResolution()
               .Include(x=>x.CourseTags).ThenInclude(x=>x.Tag).
                Include(x=>x.CourseCategory).   
                Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();


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
            ViewBag.Tag = await _context.Tag.Where(x => !x.IsDeleted).ToListAsync();


            Course? UpdateCourse = await _context.Courses.
                
                Include(x => x.CourseTags).ThenInclude(x => x.Tag)
                .Include(x=>x.CourseCategory).
                Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();

            if (course is null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (course.FormFile !=null)
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
                UpdateCourse.Image = course.FormFile.CreateImage(_environment.WebRootPath, "assets/img");
            }


            foreach (var item in course.TagIds)
            {
                if (_context.CourseTags.Where(x => x.CourseId == id &&
                   x.TagId == item).Count() > 0)
                {
                    continue;
                }
                else
                {
                    await _context.CourseTags.AddAsync(new CourseTag
                    {
                        CourseId = id,
                        TagId = item
                    });
                }

            }
            UpdateCourse.ApplyText=course.ApplyText;
            UpdateCourse.AboutText=course.AboutText;
            UpdateCourse.Certification = course.Certification;
            UpdateCourse.Name=course.Name;
            UpdateCourse.Info=course.Info;
            UpdateCourse.UpdatedDate=course.UpdatedDate;    
            UpdateCourse.CourseCategoryId=course.CourseCategoryId;


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
