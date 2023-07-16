using EduHome.App.Context;
using EduHome.App.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Controllers
{
    public class BlogController : Controller
    {
        private readonly EduHomeDbContext _context;
        public BlogController(EduHomeDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            HomeViewModel homeViewModel = new HomeViewModel();
            homeViewModel.Blogs = await _context.Blogs.
            
              Where(x => !x.IsDeleted).ToListAsync();
            homeViewModel.SocialMedias = await _context.SocialMedias.Where(x => !x.IsDeleted).ToListAsync();

            return View(homeViewModel);
        }
    }
}
