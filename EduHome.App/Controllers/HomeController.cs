
using EduHome.App.Context;
using EduHome.App.ViewModels;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace EduHome.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly EduHomeDbContext _context;
        public HomeController(EduHomeDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            HomeViewModel homeViewModel = new HomeViewModel();

            homeViewModel.Abouts = await _context.Abouts.Where(x => !x.IsDeleted).ToListAsync();
            homeViewModel.Sliders = await _context.Slides.Where(x => !x.IsDeleted).ToListAsync();
            return View(homeViewModel);
        }

     
    }
}