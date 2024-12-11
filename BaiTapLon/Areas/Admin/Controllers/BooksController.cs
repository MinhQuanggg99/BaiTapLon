﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BaiTapLon.Data;
using BaiTapLon.Models;
using BaiTapLon.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace BaiTapLon.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class BooksController : Controller
    {
        private readonly BTLContext _context;

        public BooksController(BTLContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            var BTLContext = _context.Books.Include(b => b.BookType);
            return View(await BTLContext.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var books = await _context.Books
                .Include(b => b.BookType)
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (books == null)
            {
                return NotFound();
            }

            return View(books);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["BookTypeId"] = new SelectList(_context.BookType, "BookTypeId", "BookTypeName");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookVM model)
        {
            if (ModelState.IsValid)
            {
                var books = new Books();
                books.BookName = model.BookName;
                books.Author = model.Author;
                books.BookTypeId = model.BookTypeId;
                books.BookType = model.BookType;
                books.Description = model.Description;
                books.Publisher = model.Publisher;
                books.Quantity = model.Quantity;
                var fileName = model.Image.FileName;
                var filePath = Path.Combine(Directory.GetCurrentDirectory() + "/wwwroot/Upload", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Image.CopyToAsync(stream);
                }
                books.Image = "~/Upload/" + fileName;
                _context.Add(books);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookTypeId"] = new SelectList(_context.BookType, "BookTypeId", "BookTypeName", model.BookTypeId);
            return View(model);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var books = await _context.Books.FindAsync(id);
            if (books == null)
            {
                return NotFound();
            }
            ViewData["BookTypeId"] = new SelectList(_context.BookType, "BookTypeId", "BookTypeName", books.BookTypeId);
            return View(books);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookId,BookName,Author,Publisher,BookTypeId,Quantity,Description,Image")] Books books)
        {
            if (id != books.BookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(books);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BooksExists(books.BookId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookTypeId"] = new SelectList(_context.BookType, "BookTypeId", "BookTypeName", books.BookTypeId);
            return View(books);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var books = await _context.Books
                .Include(b => b.BookType)
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (books == null)
            {
                return NotFound();
            }

            return View(books);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var books = await _context.Books.FindAsync(id);
            if (books != null)
            {
                _context.Books.Remove(books);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BooksExists(int id)
        {
            return _context.Books.Any(e => e.BookId == id);
        }
    }
}
