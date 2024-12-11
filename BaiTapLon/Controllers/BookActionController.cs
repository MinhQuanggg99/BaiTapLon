using System.Diagnostics;
using System.Security.Claims;
using BaiTapLon.Data;
using BaiTapLon.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BaiTapLon.Controllers
{
    public class BookActionController : Controller
    {

        private readonly BTLContext _context;
        private readonly UserManager<AppUser> _userManager;

        public BookActionController(BTLContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> BorrowBook(BookAction model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            var user = await _userManager.FindByIdAsync(userId);
            var book = _context.Books.SingleOrDefault(e => e.BookId == model.BookId);
            if (book == null)
            {
                return NotFound("Không tìm thấy sách.");
            }
            if (book.Quantity <= 0)
            {
                return BadRequest("Sách đã hết.");
            }
            try
            {
                var bookAction = new BookAction
                {
                    UserId = userId,
                    User = user,
                    BookId = model.BookId,
                    Book = book,
                    FullName = model.FullName,
                    ReturnDateExpected = model.ReturnDateExpected,
                };
                _context.BookAction.Add(bookAction);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", "Home", new { id = model.BookId });
            }
            catch (Exception ex)
            {
                return BadRequest($"Đã xảy ra lỗi: {ex.Message}");
            }
        }

    }
}