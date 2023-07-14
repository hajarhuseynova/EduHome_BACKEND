using EduHome.App.Context;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.areas.Admin.Controllers
{
    [Area("Admin")]
    public class NoticeBoardController : Controller
    {
        private readonly EduHomeDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public NoticeBoardController(EduHomeDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;

        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<NoticeBoard> noticeBoards = await _context.NoticeBoards.
                Where(x => !x.IsDeleted).ToListAsync();
            return View(noticeBoards);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NoticeBoard noticeBoard)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }
            noticeBoard.CreatedDate = DateTime.Now;
            await _context.AddAsync(noticeBoard);

            await _context.SaveChangesAsync();
            return RedirectToAction("index", "noticeboard");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            NoticeBoard? noticeBoard = await _context.NoticeBoards.
                  Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (noticeBoard == null)
            {
                return NotFound();
            }
            return View(noticeBoard);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, NoticeBoard noticeBoard)
        {
            NoticeBoard? UpdatedNoticeBoard = await _context.NoticeBoards.
                 Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (noticeBoard == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(UpdatedNoticeBoard);
            }
            UpdatedNoticeBoard.Description = noticeBoard.Description;
          
            UpdatedNoticeBoard.VideoLink = noticeBoard.VideoLink;
            UpdatedNoticeBoard.UpdatedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }


        public async Task<IActionResult> Delete(int id)
        {
            NoticeBoard? noticeBoard = await _context.NoticeBoards.
                Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (noticeBoard == null)
            {
                return NotFound();
            }

            noticeBoard.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
