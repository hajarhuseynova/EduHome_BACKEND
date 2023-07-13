using EduHome.App.Context;
using Microsoft.AspNetCore.Mvc;

namespace EduHome.App.areas.Admin.Controllers
{
    [Area("Admin")]
    public class CourseController : Controller
    {
        private readonly EduHomeDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public CourseController(EduHomeDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;

        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
