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
    public class TestinomialController : Controller
    {
       
        private readonly EduHomeDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public TestinomialController(EduHomeDbContext context, IWebHostEnvironment environment)
            {
                _context = context;
                _environment = environment;

            }
        public async Task<IActionResult> Index(int page=1)
        {
            int TotalCount = _context.Testinomials.Where(x => !x.IsDeleted).Count();
            ViewBag.TotalPage = (int)Math.Ceiling((decimal)TotalCount / 5);

            IEnumerable<Testinomial> testinomials = await _context.Testinomials.
                Include(x => x.PositionCategory).
                Include(x=>x.CourseCategory).
                Where(x => !x.IsDeleted).Skip((page - 1) * 5).Take(5).ToListAsync();
            return View(testinomials);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.CourseCategory = await _context.CourseCategories.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.PositionCategory = await _context.PositionCategories.Where(x => !x.IsDeleted).ToListAsync();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Testinomial testinomial)
        {
            ViewBag.CourseCategory = await _context.CourseCategories.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.PositionCategory = await _context.PositionCategories.Where(x => !x.IsDeleted).ToListAsync();

            if (!ModelState.IsValid)
            {
                return View();
            }
            if (testinomial.FormFile is null)
            {
                ModelState.AddModelError("file", "Image is required");
                return View();
            }


            if (!Helper.isImage(testinomial.FormFile))
            {
                ModelState.AddModelError("file", "Image is required");
                return View();
            }
            if (!Helper.isSizeOk(testinomial.FormFile, 1))
            {
                ModelState.AddModelError("file", "Image size is wrong");
                return View();
            }

            testinomial.CourseCategory = await _context.CourseCategories.Where(x => x.Id == testinomial.CourseCategoryId).FirstOrDefaultAsync();
            testinomial.PositionCategory = await _context.PositionCategories.Where(x => x.Id == testinomial.PositionCategoryId).FirstOrDefaultAsync();

            testinomial.CreatedDate = DateTime.Now;
            testinomial.Image = testinomial.FormFile.CreateImage(_environment.WebRootPath, "assets/img/");
            await _context.AddAsync(testinomial);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "testinomial");
        }

        public async Task<IActionResult> Update(int id)
        {

            ViewBag.CourseCategory = await _context.CourseCategories.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.PositionCategory = await _context.PositionCategories.Where(x => !x.IsDeleted).ToListAsync();

            Testinomial? testinomial = await _context.Testinomials.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (testinomial is null)
            {
                return NotFound();
            }
            return View(testinomial);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Testinomial testinomial)
        {
            ViewBag.CourseCategory = await _context.CourseCategories.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.PositionCategory = await _context.PositionCategories.Where(x => !x.IsDeleted).ToListAsync();
            Testinomial? UpdateTes = await _context.Testinomials.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();

            if (testinomial is null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (testinomial.FormFile is not null)
            {
                if (!Helper.isImage(testinomial.FormFile))
                {
                    ModelState.AddModelError("file", "Image is required");
                    return View();
                }
                if (!Helper.isSizeOk(testinomial.FormFile, 1))
                {
                    ModelState.AddModelError("file", "Image size is wrong");
                    return View();
                }
                UpdateTes.Image = testinomial.FormFile.CreateImage(_environment.WebRootPath, "assets/img/");
            }


            UpdateTes.UpdatedDate = DateTime.Now;
            UpdateTes.Desc = testinomial.Desc;
            UpdateTes.PersonName = testinomial.PersonName;
            UpdateTes.CourseCategoryId = testinomial.CourseCategoryId;
            UpdateTes.PositionCategoryId = testinomial.PositionCategoryId;


            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Testinomial? testinomial = await _context.Testinomials.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();


            if (testinomial is null)
            {
                return NotFound();
            }
            testinomial.IsDeleted = true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


    }
}
