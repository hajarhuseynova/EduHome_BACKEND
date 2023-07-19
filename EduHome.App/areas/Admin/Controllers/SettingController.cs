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
    public class SettingController : Controller
    {
       
            private readonly EduHomeDbContext _context;
            private readonly IWebHostEnvironment _environment;

            public SettingController(EduHomeDbContext context, IWebHostEnvironment environment)
            {
                _context = context;
                _environment = environment;

            }
            public async Task<IActionResult> Index(int page=1)
             {
            int TotalCount = _context.Settings.Where(x => !x.IsDeleted).Count();
            ViewBag.TotalPage = (int)Math.Ceiling((decimal)TotalCount / 5);

            IEnumerable<Setting> Settings = await _context.Settings.Where(x => !x.IsDeleted).Skip((page - 1) * 5).Take(5).ToListAsync();
            return View(Settings);  
             }
        //public async Task<IActionResult> Create()
        //{
       
        //    return View();
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(Setting setting)
        //{
        
        //    if (!ModelState.IsValid)
        //    {
        //        return View();
        //    }
        //    if (setting.AboutFormFile is null)
        //    {
        //        ModelState.AddModelError("file", "Image is required");
        //        return View();
        //    }
        //    if (!Helper.isImage(setting.AboutFormFile))
        //    {
        //        ModelState.AddModelError("file", "Image is required"); 
        //        return View();
        //    }
        //    if (!Helper.isSizeOk(setting.AboutFormFile, 1))
        //    {
        //        ModelState.AddModelError("file", "Image size is wrong");
        //        return View();
        //    }
        //    if (setting.LogoFormFile is null)
        //    {
        //        ModelState.AddModelError("file", "Image is required");
        //        return View();
        //    }

        //    if (!Helper.isImage(setting.LogoFormFile))
        //    {
        //        ModelState.AddModelError("file", "Image is required");
        //        return View();
        //    }
        //    if (!Helper.isSizeOk(setting.LogoFormFile, 1))
        //    {
        //        ModelState.AddModelError("file", "Image size is wrong");
        //        return View();
        //    }
        //    setting.CreatedDate = DateTime.Now;
        //    setting.ButtonLink = "haha";
        //    setting.VideoLink = "huhu";
        //    setting.Desc1 = "hehe";
        //    setting.Title = "hihi";
        //    setting.Desc2 = "yeap";
        //    setting.AboutImage = "lala";
        //    setting.Logo = "lala";

        //    await _context.AddAsync(setting);
        //    await _context.SaveChangesAsync();
        //    return Json("ok");
        //}

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Setting? setting = await _context.Settings.Where(c => !c.IsDeleted && c.Id == id).FirstOrDefaultAsync();
            if (setting == null)
            {
                return NotFound();
            }
            return View(setting);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Setting setting)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Setting? set = await _context.Settings.Where(c => !c.IsDeleted && c.Id == id).FirstOrDefaultAsync();
            if (set == null)
            {
                return NotFound();
            }
            if (setting.AboutFormFile != null)
            {
                if (!Helper.isImage(setting.AboutFormFile))
                {
                    ModelState.AddModelError("file", "Image is required");
                    return View();
                }
                if (!Helper.isSizeOk(setting.AboutFormFile, 1))
                {
                    ModelState.AddModelError("file", "Image size is wrong");
                    return View();
                }
                set.AboutImage = setting.AboutFormFile.CreateImage(_environment.WebRootPath, "assets/img");
            }
            if (setting.LogoFormFile != null)
            {
                if (!Helper.isImage(setting.LogoFormFile))
                {
                    ModelState.AddModelError("file", "Image is required");
                    return View();
                }
                if (!Helper.isSizeOk(setting.LogoFormFile, 1))
                {
                    ModelState.AddModelError("file", "Image size is wrong");
                    return View();
                }
                set.Logo = setting.LogoFormFile.CreateImage(_environment.WebRootPath, "assets/img");
            }

            set.UpdatedDate = DateTime.Now;
            set.Title = setting.Title;
            set.Phone1 = setting.Phone1;
            set.Phone2 = setting.Phone2;

            set.Email1 = setting.Email1;
            set.Email2 = setting.Email2;

            set.Link1 = setting.Link1;
            set.Link2 = setting.Link2;
            set.Link3 = setting.Link3;
            set.Link4 = setting.Link4;

            set.Icon1 = setting.Icon1;
            set.Icon2 = setting.Icon2;
            set.Icon3 = setting.Icon3;
            set.Icon4 = setting.Icon4;


            set.CityCountry = setting.CityCountry;
            set.Address = setting.Address;

            set.Desc2 = setting.Desc2;
            set.Desc1 = setting.Desc1;  

            set.VideoLink = setting.VideoLink;
            set.ButtonLink = setting.ButtonLink;
       
            await _context.SaveChangesAsync();
            return RedirectToAction("index", "setting");
        }






    }
}
