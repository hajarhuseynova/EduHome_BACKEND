using EduHome.App.Context;
using EduHome.App.Extentions;
using EduHome.App.Helpers;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class SkillsController : Controller
    {
       
            private readonly EduHomeDbContext _context;
            private readonly IWebHostEnvironment _environment;

            public SkillsController(EduHomeDbContext context, IWebHostEnvironment environment)
            {
                _context = context;
                _environment = environment;

            }
        public async Task<IActionResult> Index(int page=1)
        {
            int TotalCount = _context.Skills.Where(x => !x.IsDeleted).Count();
            ViewBag.TotalPage = (int)Math.Ceiling((decimal)TotalCount / 5);

            IEnumerable<Skills> skills = await _context.Skills.Include(x=>x.Teacher).
                Where(x => !x.IsDeleted).Skip((page - 1) * 5).Take(5).ToListAsync();
            return View(skills);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Teacher= await _context.Teachers.Where(x => !x.IsDeleted).ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(Skills skills)
        {
            ViewBag.Teacher = await _context.Teachers.Where(x => !x.IsDeleted).ToListAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }

            skills.CreatedDate = DateTime.Now;
            await _context.AddAsync(skills);

            await _context.SaveChangesAsync();
            return RedirectToAction("index", "skills");

        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
           Skills? skills = await _context.Skills.
            Where(x => !x.IsDeleted&&x.Id==id).FirstOrDefaultAsync();

            ViewBag.Teacher = await _context.Teachers.Where(x => !x.IsDeleted).ToListAsync();

            if (skills == null)
            {
                return NotFound();
            }
            return View(skills);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Skills skills)
        {
            ViewBag.Teacher = await _context.Teachers.Where(x => !x.IsDeleted).ToListAsync();


            Skills? Update = await _context.Skills.Include(x => x.Teacher).
        Where(x => !x.IsDeleted&&x.Id==id).FirstOrDefaultAsync();


            if (skills == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(Update);
            }


            Update.Name = skills.Name;
            Update.Percent= skills.Percent;   
            Update.UpdatedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Delete(int id)
        {
            Skills? skills = await _context.Skills.
            Where(x => !x.IsDeleted&&x.Id==id).FirstOrDefaultAsync();

            if (skills == null)
            {
                return NotFound();
            }
            skills.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }




}
