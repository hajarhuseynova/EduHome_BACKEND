using EduHome.App.Context;
using EduHome.App.Extentions;
using EduHome.App.Helpers;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.areas.Admin.Controllers
{
        [Area("Admin")]
    public class BlogController : Controller
    {
       
        private readonly EduHomeDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public BlogController(EduHomeDbContext context, IWebHostEnvironment environment)
            {
                _context = context;
                _environment = environment;

            }
        public async Task<IActionResult> Index(int page = 1)
        {
            int TotalCount = _context.Courses.Where(x => !x.IsDeleted).Count();
            ViewBag.TotalPage = (int)Math.Ceiling((decimal)TotalCount / 5);

            IEnumerable<Blog> Blogs = await _context.Blogs.
                   Include(x => x.CourseCategory).
                Where(x => !x.IsDeleted).Skip((page - 1) * 5).Take(5).ToListAsync();
            return View(Blogs);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.CourseCategory = await _context.CourseCategories.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Tag = await _context.Tag.Where(x => !x.IsDeleted).ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(Blog blog)
        {
            ViewBag.CourseCategory = await _context.CourseCategories.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Tag = await _context.Tag.Where(x => !x.IsDeleted).ToListAsync();

            if (!ModelState.IsValid)
            {
                return View();
            }
            if (blog.FormFile == null)
            {
                ModelState.AddModelError("FormFile", "Wrong!");
                return View();
            }
            if (!Helper.isImage(blog.FormFile))
            {
                ModelState.AddModelError("FormFile", "Wronggg!");
                return View();
            }
            if (!Helper.isSizeOk(blog.FormFile, 1))
            {
                ModelState.AddModelError("FormFile", "Wronggg!");
                return View();
            }
            foreach (var item in blog.TagIds)
            {
                if (!await _context.Tag.AnyAsync(x => x.Id == item))
                {
                    ModelState.AddModelError("", "Wrongg!");
                    return View(blog);
                }
                BlogTag blogTag = new BlogTag
                {
                    TagId = item,
                    Blog = blog,
                    CreatedDate = DateTime.Now
                };
                await _context.BlogTags.AddAsync(blogTag);
            }
            blog.CourseCategory = await _context.CourseCategories.Where(x => x.Id == blog.CourseCategoryId).FirstOrDefaultAsync();

          
            blog.Image = blog.FormFile.CreateImage(_environment.WebRootPath, "assets/img/");
            blog.CreatedDate = DateTime.Now;
            await _context.AddAsync(blog);

            await _context.SaveChangesAsync();
            return RedirectToAction("index", "blog");

        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            ViewBag.CourseCategory = await _context.CourseCategories.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Tag = await _context.Tag.Where(x => !x.IsDeleted).ToListAsync();

            Blog? blog = await _context.Blogs.
                Where(c => !c.IsDeleted && c.Id == id).Include(x => x.BlogTags).
                ThenInclude(x => x.Tag).Include(x => x.CourseCategory)
                .FirstOrDefaultAsync();

            if (blog == null)
            {
                return NotFound();
            }
            return View(blog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Blog blog)
        {
            ViewBag.CourseCategory = await _context.CourseCategories.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Tag = await _context.Tag.Where(x => !x.IsDeleted).ToListAsync();

            Blog? Update = await _context.Blogs.
                Where(c => !c.IsDeleted && c.Id == id).Include(x=>x.BlogTags).
                ThenInclude(x => x.Tag).Include(x => x.CourseCategory)
                .FirstOrDefaultAsync();

            if (blog == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(Update);
            }

            if (blog.FormFile != null)
            {
                if (!Helper.isImage(blog.FormFile))
                {
                    ModelState.AddModelError("FormFile", "Wronggg!");
                    return View();
                }
                if (!Helper.isSizeOk(blog.FormFile, 1))
                {
                    ModelState.AddModelError("FormFile", "Wronggg!");
                    return View();
                }
              
                Update.Image = blog.FormFile.CreateImage(_environment.WebRootPath, "assets/img");
            }

            List<BlogTag> RemovableTag = await _context.BlogTags.
               Where(x => !blog.TagIds.Contains(x.TagId))
               .ToListAsync();

            _context.BlogTags.RemoveRange(RemovableTag);

            foreach (var item in blog.TagIds)
            {
                if (!await _context.Tag.AnyAsync(x => x.Id == item))
                {
                    ModelState.AddModelError("", "Wrongg!");
                    return View(blog);
                }
                BlogTag blogTag = new BlogTag
                {
                    TagId = item,
                    Blog = blog,
                    CreatedDate = DateTime.Now
                };
            }
            Update.Name = blog.Name;
            Update.Desc = blog.Desc;
            Update.Icon = blog.Icon;
            Update.Title = blog.Title;
            Update.UpdatedDate = DateTime.Now;
            Update.CourseCategoryId=blog.CourseCategoryId;  
           

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Delete(int id)
        {
            Blog? blog = await _context.Blogs.
                  Where(c => !c.IsDeleted && c.Id == id).FirstOrDefaultAsync();
            if (blog == null)
            {
                return NotFound();
            }
            blog.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }




}
