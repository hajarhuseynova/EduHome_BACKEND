using EduHome.App.Context;
using EduHome.App.Extentions;
using EduHome.App.Helpers;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactController : Controller
    {
        private readonly EduHomeDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ContactController(EduHomeDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;

        }
        public async Task<IActionResult> Index(int page = 1)
        {
            int TotalCount = _context.Courses.Where(x => !x.IsDeleted).Count();
            ViewBag.TotalPage = (int)Math.Ceiling((decimal)TotalCount / 5);

            IEnumerable<Contact> contacts =
                await _context.Contacts.Where(c => !c.IsDeleted).Skip((page - 1) * 5).Take(5).ToListAsync();
            return View(contacts);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contact Contact)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (Contact.FormFile is null)
            {
                ModelState.AddModelError("file", "Image is required");
                return View();
            }


            if (!Helper.isImage(Contact.FormFile))
            {
                ModelState.AddModelError("file", "Image is required");
                return View();
            }
            if (!Helper.isSizeOk(Contact.FormFile, 1))
            {
                ModelState.AddModelError("file", "Image size is wrong");
                return View();
            }
            Contact.CreatedDate = DateTime.Now;
            Contact.Image = Contact.FormFile.CreateImage(_environment.WebRootPath, "assets/img/");

            await _context.AddAsync(Contact);
            await _context.SaveChangesAsync();
            return RedirectToAction("index", "Contact");
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Contact? Contact = await _context.Contacts.Where(c => !c.IsDeleted && c.Id == id).FirstOrDefaultAsync();
            if (Contact == null)
            {
                return NotFound();
            }
            return View(Contact);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Contact Contact)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Contact? fac = await _context.Contacts.Where(c => !c.IsDeleted && c.Id == id).FirstOrDefaultAsync();
            if (fac == null)
            {
                return NotFound();
            }
            if (Contact.FormFile != null)
            {
                if (!Helper.isImage(Contact.FormFile))
                {
                    ModelState.AddModelError("file", "Image is required");
                    return View();
                }
                if (!Helper.isSizeOk(Contact.FormFile, 1))
                {
                    ModelState.AddModelError("file", "Image size is wrong");
                    return View();
                }
                fac.Image = Contact.FormFile.CreateImage(_environment.WebRootPath, "assets/img");
            }


            fac.UpdatedDate = DateTime.Now;
        
            fac.Address = Contact.Address;
            fac.CityContry = Contact.CityContry;


            await _context.SaveChangesAsync();
            return RedirectToAction("index", "Contact");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Contact? Contact = await _context.Contacts.Where(c => !c.IsDeleted && c.Id == id).FirstOrDefaultAsync();
            if (Contact == null)
            {
                return NotFound();
            }
            Contact.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
