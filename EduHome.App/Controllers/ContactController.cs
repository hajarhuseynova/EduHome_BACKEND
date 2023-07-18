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
            if(name==null || subject==null || message==null || email==null)
            {
            return RedirectToAction("index", "contact");

            }
            SendMessage mes = new SendMessage
            {
                Name = name,
                Email = email,
                Message = message,
                Subject = subject,
                CreatedDate=DateTime.Now
            };
            _context.SendMessages.Add(mes);
            _context.SaveChanges();
            return RedirectToAction("index", "contact");
        }

    }
}
