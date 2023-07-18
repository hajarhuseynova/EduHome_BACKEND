using EduHome.App.Context;
using EduHome.App.ViewModels;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Controllers
{
    public class ContactController : Controller
    {
        private readonly EduHomeDbContext _context;
        public ContactController(EduHomeDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            //HomeViewModel homeViewModel = new HomeViewModel();
            //homeViewModel.Settings = await _context.Settings.Where(x => !x.IsDeleted).ToListAsync();

            return View();
        }

        public async Task<IActionResult> SendMessage(string name, string subject, string message, string email)
        {
            SendMessage mes = new SendMessage
            {
                Name = name,
                Email = email,
                Message = message,
                Subject = subject
            };
            _context.Add(mes);
            return RedirectToAction("index", "contact");
        }

    }
}
