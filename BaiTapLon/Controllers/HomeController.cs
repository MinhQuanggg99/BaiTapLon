using System.Diagnostics;
using BaiTapLon.Data;
using BaiTapLon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BaiTapLon.Controllers
{
    public class HomeController : Controller
    {

        private readonly BTLContext _context;

        public HomeController(BTLContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Product(int? id)
        {
            if (id.HasValue)
            {
                var model = await _context.Books.Where(e => e.BookTypeId == id).ToListAsync();
                return View(model);
            }

            var allBooks = await _context.Books.ToListAsync();
            return View(allBooks);
        }
        public IActionResult About()
        {
            return View();
        }
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            var model = await _context.Books.Where(e => e.BookId == id).Include(e => e.BookType).FirstOrDefaultAsync();
            return View(model);
        }
        [Authorize]
        public async Task<IActionResult> Borrow(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            var model = await _context.Books.Where(e => e.BookId == id).Include(e => e.BookType).FirstOrDefaultAsync();
            return View(model);
        }

    }
}
