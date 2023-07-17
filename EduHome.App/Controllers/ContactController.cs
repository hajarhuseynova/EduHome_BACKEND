using EduHome.App.Context;
using EduHome.App.Services.Interfaces;
using EduHome.App.ViewModels;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
            ContactViewModel ContactViewModel = new ContactViewModel
            {
                Contacts = await _context.Contacts
                        .Where(x => !x.IsDeleted).ToListAsync(),
            
            };

            return View(ContactViewModel);
        }



    }

}
