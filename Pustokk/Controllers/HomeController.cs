using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustokk.DAL;
using Pustokk.ViewModels;
using System.Diagnostics;

namespace PustokTask1.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            HomeViewModel homeViewModel = new HomeViewModel()
            {
                Sliders = _context.Sliders.ToList(),
                Services = _context.Services.ToList(),
                FeaturedBooks = _context.Books.Include(x=>x.Author).Include(x=>x.BookImages).Where(x => x.IsFeatured == true).ToList(),
                NewBooks = _context.Books.Include(x => x.Author).Include(x => x.BookImages).Where(x => x.IsNew == true).ToList(),
                BestSellerBooks = _context.Books.Include(x => x.Author).Include(x => x.BookImages).Where(x => x.IsBestSeller == true).ToList(),

            };

            return View(homeViewModel);
        
        }
    }
}