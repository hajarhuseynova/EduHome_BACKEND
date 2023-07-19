using EduHome.App.Context;
using EduHome.App.ViewModels;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

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

            ContactViewModel contactViewModel = new ContactViewModel();
            contactViewModel.Setting = await _context.Settings.Where(x => !x.IsDeleted).FirstOrDefaultAsync();
            contactViewModel.Message = await _context.SendMessages.Where(x => !x.IsDeleted).FirstOrDefaultAsync();

            return View(contactViewModel);
        }

        public async Task<IActionResult> SendMessage(string name, string subject, string message, string email)
        {
            if(name==null || subject==null || message==null || email==null)
            {
                TempData["ContactFalse"] = "Please,fill the all gab!";
                return RedirectToAction("index", "contact");

            }
            Regex regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!regex.IsMatch(email))
            {
                TempData["EmailReg"] = "Email must be true version!";
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
            TempData["ContactTrue"] = "Successfully send!";
            return RedirectToAction("index", "contact");
        }

    }
}
