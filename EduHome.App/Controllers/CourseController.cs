using EduHome.App.Context;
using EduHome.App.ViewModels;
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
        public async Task<IActionResult> Index()
        {
            CourseViewModel courseViewModel = new CourseViewModel
            {
                Courses = await _context.Courses
                       .Include(x => x.Feature)
                        .Include(x => x.CourseTags).ThenInclude(x=>x.Tag)
                       .Include(x => x.CourseCategory).Where(x => !x.IsDeleted)
                       .ToListAsync(),
                           Course = await _context.Courses
                       .Include(x => x.Feature)
                        .Include(x => x.CourseTags).ThenInclude(x => x.Tag)
                       .Include(x => x.CourseCategory).Where(x => !x.IsDeleted)
                       .FirstOrDefaultAsync()

            };

            return View(courseViewModel);
        }
        public async Task<IActionResult> Detail(int id)
        {
            CourseViewModel courseViewModel = new CourseViewModel
            {
                Courses = await _context.Courses
                       .Include(x => x.Feature)
                        .Include(x => x.CourseTags).ThenInclude(x => x.Tag)
                       .Include(x => x.CourseCategory).Where(x => !x.IsDeleted)
                       .ToListAsync(),
                Course = await _context.Courses
                       .Include(x => x.Feature)
                        .Include(x => x.CourseTags).ThenInclude(x => x.Tag)
                       .Include(x => x.CourseCategory).Where(x => !x.IsDeleted&&x.Id==id)
                       .FirstOrDefaultAsync()



            };

            return View(courseViewModel);
        }
    }
}
