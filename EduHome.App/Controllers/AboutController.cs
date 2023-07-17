using EduHome.App.Context;
using EduHome.App.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Controllers
{
    public class AboutController : Controller
    {
        private readonly EduHomeDbContext _context;
        public AboutController(EduHomeDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            HomeViewModel homeViewModel = new HomeViewModel();
            homeViewModel.Teachers = await _context.Teachers.
              Include(x => x.teacherPositionCat).Include(x => x.SocialMedias).
              Where(x => !x.IsDeleted).ToListAsync();
            homeViewModel.SocialMedias = await _context.SocialMedias.Where(x => !x.IsDeleted).ToListAsync();
            homeViewModel.NoticeBoards = await _context.NoticeBoards.Where(x => !x.IsDeleted).ToListAsync();
            homeViewModel.Settings = await _context.Settings.Where(x => !x.IsDeleted).ToListAsync();




            homeViewModel.Testinomials = await _context.Testinomials.Include(x => x.CourseCategory).Include(x => x.PositionCategory)
              .Where(x => !x.IsDeleted).ToListAsync();

            return View(homeViewModel);
        }
    }
}
