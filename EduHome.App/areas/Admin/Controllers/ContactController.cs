using EduHome.App.Context;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.areas.Admin.Controllers
{
   [Area("Admin")]
    public class ContactController : Controller
    {
        private readonly EduHomeDbContext _context;
        public ContactController(EduHomeDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<SendMessage> mes = await _context.SendMessages.Where(x =>!x.IsDeleted).ToListAsync();
            return View(mes);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            SendMessage? mes = await _context.SendMessages.Where(x => !x.IsDeleted&&x.Id == id).FirstOrDefaultAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (mes == null)
            {
                return NotFound();
            }
            mes.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction("index", "contact");
        }
    }
}
