using EduHome.App.Context;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.areas.Admin.Controllers
{
    [Area("Admin")]
    public class FeatureController : Controller
    {
        private readonly EduHomeDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public FeatureController(EduHomeDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;

        }
        public async Task<IActionResult> Index(int page=1)
        {
            int TotalCount = _context.Feature.Where(x => !x.IsDeleted).Count();
            ViewBag.TotalPage = (int)Math.Ceiling((decimal)TotalCount / 5);

            IEnumerable<Feature> features =
                await _context.Feature.Include(x=>x.Course).Where(c => !c.IsDeleted).Skip((page - 1) * 5).Take(5).ToListAsync();
            return View(features);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Course = await _context.Courses.Where(x => !x.IsDeleted&& x.Feature==null).ToListAsync();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Feature feature)
        {
            ViewBag.Course = await _context.Courses.Where(x => !x.IsDeleted && x.Feature == null).ToListAsync();

            if (!ModelState.IsValid)
            {
                return View();
            }
            feature.CreatedDate = DateTime.Now;
            await _context.AddAsync(feature);
            await _context.SaveChangesAsync();
            return RedirectToAction("index", "feature");
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            ViewBag.Course = await _context.Courses.Where(x => !x.IsDeleted).ToListAsync();

            Feature? feature = await _context.Feature.Where(c => !c.IsDeleted && c.Id == id).FirstOrDefaultAsync();
            if (feature == null)
            {
                return NotFound();
            }
            return View(feature);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Feature feature)
        {
            ViewBag.Course = await _context.Courses.Where(x => !x.IsDeleted).ToListAsync();

            Feature? fec = await _context.Feature
                .Where(c => !c.IsDeleted && c.Id == id).FirstOrDefaultAsync();

            if (!ModelState.IsValid)
            {
                return View();
            }
            if (fec == null)
            {
                return NotFound();
            }

            fec.Duration = feature.Duration;
            fec.StartTime=feature.StartTime;
            fec.UpdatedDate = DateTime.Now;
            fec.ClassDuration = feature.ClassDuration;
            fec.StudentCount = feature.StudentCount;
            fec.Fee = feature.Fee;
            fec.SkillLevel = feature.SkillLevel;
            fec.Language = feature.Language;
            fec.Assesments = feature.Assesments;
            fec.CourseId = feature.CourseId;
          
            await _context.SaveChangesAsync();
            return RedirectToAction("index", "feature");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Feature? feature = await _context.Feature.Where(c => !c.IsDeleted && c.Id == id).FirstOrDefaultAsync();
            if (feature == null)
            {
                return NotFound();
            }
            feature.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
