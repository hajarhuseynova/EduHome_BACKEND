using EduHome.App.Context;
using EduHome.App.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Controllers
{
    public class BlogController : Controller
    {
        private readonly EduHomeDbContext _context;
        public BlogController(EduHomeDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int? id = null,int page=1)
        {
            int TotalCount = _context.Blogs.Where(x => !x.IsDeleted).Count();
            ViewBag.TotalPage = (int)Math.Ceiling((decimal)TotalCount / 6);
            ViewBag.CurrentPage = page;
            if (id == null)
            {
                BlogViewModel blogViewModel = new BlogViewModel
                {
                    Blogs = await _context.Blogs
                       .Include(x => x.BlogTags)
                        .ThenInclude(x => x.Tag)
                       .Include(x => x.CourseCategory)
                        .Where(x => !x.IsDeleted).Skip((page - 1) * 6).Take(6).ToListAsync(),

                    Categories = await _context.CourseCategories.Include(x=>x.Blogs.Where(x => !x.IsDeleted)).Where(b => !b.IsDeleted).ToListAsync(),
                    Tags = await _context.Tag.Where(b => !b.IsDeleted).ToListAsync(),


                };

                return View(blogViewModel);
            }
            else
            {
                BlogViewModel blogViewModel = new BlogViewModel
                {
                    Blogs = await _context.Blogs
                       .Include(x => x.BlogTags)
                        .ThenInclude(x => x.Tag)
                       .Include(x => x.CourseCategory)
                        .Where(x => !x.IsDeleted && x.CourseCategoryId == id).ToListAsync(),

                    Categories = await _context.CourseCategories.Include(x => x.Blogs.Where(x=>!x.IsDeleted)).Where(b => !b.IsDeleted).ToListAsync(),
                    Tags = await _context.Tag.Where(b => !b.IsDeleted).ToListAsync(),


                };

                return View(blogViewModel);

            }
        }

        public async Task<IActionResult> Detail(int id)
        {
            BlogViewModel blogViewModel = new BlogViewModel
            {
                Blogs = await _context.Blogs
                       .Include(x => x.BlogTags)
                        .ThenInclude(x => x.Tag)
                       .Include(x => x.CourseCategory)
                        .Where(x => !x.IsDeleted).ToListAsync(),
                Blog = await _context.Blogs
                       .Include(x => x.BlogTags)
                        .ThenInclude(x => x.Tag)
                       .Include(x => x.CourseCategory)
                        .Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync(),

                Categories = await _context.CourseCategories.Include(x => x.Blogs.Where(x => !x.IsDeleted)).Where(b => !b.IsDeleted).ToListAsync(),
                Tags = await _context.Tag.Where(b => !b.IsDeleted).ToListAsync(),

            };
            if (blogViewModel.Blog == null)
            {
                return View(nameof(Index));
            }


            return View(blogViewModel);
        }
    }
}
