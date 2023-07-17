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
        public async Task<IActionResult> Index(int? id = null)
        {
            if (id == null)
            {
                BlogViewModel blogViewModel = new BlogViewModel
                {
                    Blogs = await _context.Blogs
                       .Include(x => x.BlogTags)
                        .ThenInclude(x => x.Tag)
                       .Include(x => x.CourseCategory)
                        .Where(x => !x.IsDeleted).ToListAsync(),

                    BlogTags= await _context.BlogTags.Include(x=>x.Tag).Where(b=>!b.IsDeleted).ToListAsync(), 
                 
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



                    BlogTags= await _context.BlogTags.Include(x=>x.Tag).Where(b=>!b.IsDeleted).ToListAsync(),



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
                        .Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync()
            };

            return View(blogViewModel);
        }
    }
}
