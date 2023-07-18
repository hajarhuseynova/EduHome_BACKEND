using EduHome.App.Context;
using EduHome.App.Extentions;
using EduHome.App.Helpers;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.areas.Admin.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class SubscribeController : Controller
    {
       
        private readonly EduHomeDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public SubscribeController(EduHomeDbContext context, IWebHostEnvironment environment)
            {
                _context = context;
                _environment = environment;

            }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Subscribe> sub = await _context.Subscribes.Where(x =>!x.IsDeleted).ToListAsync();
            return View(sub);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Subscribe? sub = await _context.Subscribes.Where(x =>!x.IsDeleted&&x.Id == id).FirstOrDefaultAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (sub == null)
            {
                return NotFound();
            }
            sub.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction("index", "subscribe");
        }
    }

}
