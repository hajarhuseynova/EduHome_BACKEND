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
            HomeViewModel homeViewModel = new HomeViewModel();

            homeViewModel.Courses = await _context.Courses.
            Include(x => x.CourseCategory).Include(x => x.CourseTags).ThenInclude(x => x.Tag).
            Where(x => !x.IsDeleted).ToListAsync();

            homeViewModel.SocialMedias = await _context.SocialMedias.Where(x => !x.IsDeleted).ToListAsync();

            return View(homeViewModel);
        }
    }
}
