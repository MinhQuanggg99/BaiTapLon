using System.Diagnostics;
using System.Security.Claims;
using BaiTapLon.Data;
using BaiTapLon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BaiTapLon.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BookActionController : Controller
    {

        private readonly BTLContext _context;

        public BookActionController(BTLContext context, UserManager<AppUser> userManager)
        {
            _context = context;
        }
        public async Task<IActionResult> BorrowBook()
        {
            var model = await _context.BookAction.Where(e => e.ActionStatus == "Đang xử lý" || e.ActionStatus == "Đang mượn").Include(e => e.Book).ToListAsync();
            return View(model);
        }
        public async Task<IActionResult> ReturnBook()
        {
            var model = await _context.BookAction.Where(e => e.ActionStatus == "Đã trả").Include(e => e.Book).ToListAsync();
            return View(model);
        }

        public async Task<IActionResult> ConfirmReturn(int id)
        {
            var request = _context.BookAction.Where(e => e.BookActionId == id).SingleOrDefault();
            if (request == null)
            {
                return NotFound();
            }
            request.ActionStatus = "Đã trả";
            _context.BookAction.Update(request);
            var book = _context.Books.Where(e => e.BookId == request.BookId).SingleOrDefault();
            if (book != null)
            {
                book.Quantity += 1;
                _context.Books.Update(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("ReturnBook");

        }
        public async Task<IActionResult> Confirmrefuse(int id)
        {
            var request = _context.BookAction.Where(e => e.BookActionId == id).SingleOrDefault();
            if (request == null)
            {
                return NotFound();
            }
            _context.BookAction.Remove(request);
            await _context.SaveChangesAsync();
            return RedirectToAction("BorrowBook");

        }
        public async Task<IActionResult> ConfirmBorrow(int? id)
        {
            var request = _context.BookAction.Where(e => e.BookActionId == id).SingleOrDefault();
            if (request == null)
            {
                return NotFound();
            }
            request.ActionStatus = "Đang mượn";
            _context.BookAction.Update(request);
            var book = _context.Books.Where(e => e.BookId == request.BookId).SingleOrDefault();
            if (book != null)
            {
                book.Quantity -= 1;
                _context.Books.Update(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("BorrowBook");
        }

    }
}