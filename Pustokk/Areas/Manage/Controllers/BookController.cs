using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustokk.DAL;
using Pustokk.Extencions;
using Pustokk.Models;
using Pustokk.Services.Interfaces;
using System.Data;
using System.Linq;

namespace Pustokk.Areas.Manage.Controllers
{
    [Area("manage")]
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
           
        }
        public async Task<IActionResult> Index()
        {
            List<Book> books =await _bookService.GetAllAsync();
          
            return View(books);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Authors = await _bookService.GetAllAuthorAsync();
            ViewBag.Genres = await _bookService.GetAllGenreAsync();
            ViewBag.Tags = await _bookService.GetAllTagAsync();
            return View();
        }
        public async Task<IActionResult> Create(Book book)
        {
            ViewBag.Authors = await _bookService.GetAllAuthorAsync();
            ViewBag.Genres = await _bookService.GetAllGenreAsync();
            ViewBag.Tags = await _bookService.GetAllTagAsync();
            if (!ModelState.IsValid) return View(book);
            try
            {
                await _bookService.CreatAsync(book);
            }
            catch (Exception) { }
     

            return RedirectToAction("index");
        }
        [HttpGet]
        public async Task<IActionResult> UpdateAsync(int id)
        {

            ViewBag.Authors = await _bookService.GetAllAuthorAsync();
            ViewBag.Genres = await _bookService.GetAllGenreAsync();
            ViewBag.Tags = await _bookService.GetAllTagAsync();
            Book existbook = await _bookService.GetAsync(id);
            //Book existbook = await _bookService.GetAllAsync.Include(x => x.BookTags).Include(x => x.BookImages).FirstOrDefault(x => x.Id == id);
            if (existbook == null)
            {
                return NotFound();
            };
            existbook.TagIds = existbook.BookTags.Select(bt => bt.TagId).ToList();

            return View(existbook);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateAsync(Book book)
        {
            ViewBag.Authors = await _bookService.GetAllAuthorAsync();
            ViewBag.Genres = await _bookService.GetAllGenreAsync();
            ViewBag.Tags = await _bookService.GetAllTagAsync();

            if (!ModelState.IsValid) return View();

            return RedirectToAction("index");
        }
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (id == null) return NotFound();   
            try
            {
                await _bookService.DeleteAsync(id);
            }
            catch (Exception) { }
            return Ok();
        }
    }
}
