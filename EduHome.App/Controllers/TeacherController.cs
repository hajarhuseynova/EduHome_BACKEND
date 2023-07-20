using EduHome.App.Context;
using EduHome.App.ViewModels;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Controllers
{
    public class TeacherController : Controller
    {
        private readonly EduHomeDbContext _context;
        public TeacherController(EduHomeDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int page =1)
        {
            int TotalCount = _context.Teachers.Where(x => !x.IsDeleted).Count();
            ViewBag.TotalPage = (int)Math.Ceiling((decimal)TotalCount / 12);
            ViewBag.CurrentPage = page;
            TeacherViewModel teacherViewModel = new TeacherViewModel
            {
                Teachers = await _context.Teachers
                       .Include(x => x.Skills.Where(x => !x.IsDeleted))
                        .Include(x => x.SocialMedias)
                       .Include(x => x.Faculty)

                        .Include(x => x.teacherPositionCat)
                        .Where(x => !x.IsDeleted).Skip((page - 1) * 12).Take(12).ToListAsync(),

                Teacher = await _context.Teachers
                       .Include(x => x.Skills.Where(x => !x.IsDeleted))
                        .Include(x => x.SocialMedias)
                       .Include(x => x.Faculty)
                        .Include(x => x.teacherPositionCat)
                        .Where(x => !x.IsDeleted).FirstOrDefaultAsync()
            };

            return View(teacherViewModel);
        }
        public async Task<IActionResult> Detail(int id)
        {
            TeacherViewModel teacherViewModel = new TeacherViewModel
            {
                
                Teachers = await _context.Teachers
                       .Include(x => x.Skills.Where(x => !x.IsDeleted))
                        .Include(x => x.SocialMedias)
                       .Include(x => x.Faculty)
                        .Include(x => x.teacherPositionCat)
                        .Where(x => !x.IsDeleted).ToListAsync(),

                Teacher = await _context.Teachers
                       .Include(x => x.Skills.Where(x => !x.IsDeleted))
                        .Include(x => x.SocialMedias)
                       .Include(x => x.Faculty)
                        .Include(x => x.teacherPositionCat)
                        .Where(x => !x.IsDeleted&&x.Id==id).FirstOrDefaultAsync()
            };

            return View(teacherViewModel);
        }
    }
}
