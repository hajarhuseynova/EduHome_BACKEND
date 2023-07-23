using EduHome.App.Context;
using EduHome.App.ViewModels;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Controllers
{
    public class CourseController : Controller
    {
        private readonly EduHomeDbContext _context;

        public CourseController(EduHomeDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? id=null,int page=1)
        {
            int TotalCount = _context.Courses.Where(x => !x.IsDeleted).Count();
            ViewBag.TotalPage = (int)Math.Ceiling((decimal)TotalCount / 9);
            ViewBag.CurrentPage = page;

            if (id==null)
            {
                CourseViewModel courseViewModel = new CourseViewModel
                {
                    Courses = await _context.Courses
                    .Include(x => x.Feature).Where(x => !x.IsDeleted)
                     .Include(x => x.CourseTags).ThenInclude(x => x.Tag)
                    .Include(x => x.CourseCategory).Where(x => !x.IsDeleted).Skip((page - 1) * 9).Take(9)
                    .ToListAsync(),

                    Categories = await _context.CourseCategories.Where(b => !b.IsDeleted).ToListAsync(),
                };

                return View(courseViewModel);
            }
            else
            {
                CourseViewModel courseViewModel = new CourseViewModel
                {
                    Courses = await _context.Courses
                   .Include(x => x.Feature).Where(x => !x.IsDeleted)
                    .Include(x => x.CourseTags).ThenInclude(x => x.Tag)
                   .Include(x => x.CourseCategory).Where(x => !x.IsDeleted&&x.CourseCategoryId==id).Skip((page - 1) * 5).Take(5)
                   .ToListAsync(),

                    Categories = await _context.CourseCategories.Where(b => !b.IsDeleted).ToListAsync(),
                };

                return View(courseViewModel);

            }
        }

        public async Task<IActionResult> Detail(int id)
        {
            CourseViewModel courseViewModel = new CourseViewModel
            {
                Courses = await _context.Courses
                       .Include(x => x.Feature).Where(x => !x.IsDeleted)
                        .Include(x => x.CourseTags).ThenInclude(x => x.Tag)
                       .Include(x => x.CourseCategory).Where(x => !x.IsDeleted)
                       .ToListAsync(),
                Course = await _context.Courses
                       .Include(x => x.Feature).Where(x=>!x.IsDeleted)
                        .Include(x => x.CourseTags).ThenInclude(x => x.Tag)
                       .Include(x => x.CourseCategory).Where(x => !x.IsDeleted&&x.Id==id)
                       .FirstOrDefaultAsync(),

                    Categories = await _context.CourseCategories.Where(b => !b.IsDeleted&&b.Courses.Any(d=>!d.IsDeleted)).ToListAsync(),


            };

            return View(courseViewModel);
        }

        public async Task<IActionResult> Search(string search)
        {
            int TotalCount = _context.Courses.Where(x => !x.IsDeleted && x.Name.Trim().ToLower().Contains(search.Trim().ToLower())).Count();
         
            List<Course> courses = await _context.Courses.Where(x => !x.IsDeleted && x.CourseCategory.Name.Trim().ToLower().Contains(search.Trim().ToLower()))
                .Include(x => x.Feature)
                  .Include(x => x.CourseCategory)
           
                       .Include(x => x.CourseTags)
                 .ThenInclude(x => x.Tag)
                .ToListAsync();
            return Json(courses);
        }


    }
}
