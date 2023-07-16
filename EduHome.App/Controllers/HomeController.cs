
using EduHome.App.Context;
using EduHome.App.Services.Interfaces;
using EduHome.App.ViewModels;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace EduHome.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly EduHomeDbContext _context;

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signinManager;
        private readonly IMailService _mailService;
        public HomeController(EduHomeDbContext context, UserManager<AppUser> userManager = null, SignInManager<AppUser> signinManager = null, IMailService mailService = null)
        {
            _context = context;
            _userManager = userManager;
            _signinManager = signinManager;
            _mailService = mailService;
        }
        public async Task<IActionResult> Index()
        {
            HomeViewModel homeViewModel = new HomeViewModel();

           
            homeViewModel.Sliders = await _context.Slides.Where(x => !x.IsDeleted).ToListAsync();

            homeViewModel.CourseCategories = await _context.CourseCategories.Where(x => !x.IsDeleted).ToListAsync();

            homeViewModel.PositionCategories = await _context.PositionCategories.Where(x => !x.IsDeleted).ToListAsync();


            homeViewModel.NoticeBoards = await _context.NoticeBoards.Where(x => !x.IsDeleted).ToListAsync();
            homeViewModel.Blogs = await _context.Blogs.Where(x => !x.IsDeleted).ToListAsync();


            homeViewModel.Teachers = await _context.Teachers.
                Include(x=>x.teacherPositionCat).Include(x=>x.SocialMedias).Include(x=>x.Faculty).Include(x=>x.Skills).
                Where(x => !x.IsDeleted).ToListAsync();

           

            homeViewModel.Testinomials = await _context.Testinomials.Include(x =>x.CourseCategory).Include(x=>x.PositionCategory)
              .Where(x => !x.IsDeleted).ToListAsync();

            homeViewModel.Courses = await _context.Courses.Include(x=>x.Feature).Include(x=>x.CourseCategory).Include(x=>x.CourseTags).ThenInclude(x=>x.Tag)
                .Where(x => !x.IsDeleted).ToListAsync();

         

            return View(homeViewModel);
        }



    }
}