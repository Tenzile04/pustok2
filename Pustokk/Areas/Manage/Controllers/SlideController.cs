using Microsoft.AspNetCore.Mvc;
using Pustokk.Models;
using Pustokk.DAL;
using Pustokk.Models;
using System.Runtime.CompilerServices;
using Pustokk.Extencions;
using Microsoft.EntityFrameworkCore;
using Pustokk.Repositories.Interfaces;
using Pustokk.Services.Interfaces;
using Pustokk.Services.Implementations;
using Pustokk.CustomExceptions.SliderException;

namespace PustokTask1.Areas.Manage.Controllers
{
    [Area("manage")]
    public class SlideController : Controller
    {
        private readonly ISliderService _sliderService;
        public SlideController(ISliderService sliderService)
        {
            _sliderService = sliderService;
        }
        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = await _sliderService.GetAllAsync();
            return View(sliders);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Slider slider)
        {
            if (!ModelState.IsValid) return View();
            try
            {
                await _sliderService.CreatAsync(slider);

            }
            catch (InvalidContentTypeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (InvalidImageSizeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch(InvalidImageFileException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (Exception) { }

            return RedirectToAction("index");
        }

        public async Task<IActionResult> Update(int id)
        {
            if (id == null) return View();
            Slider wantedSlider = await _sliderService.GetAsync(id);

            if (wantedSlider == null) return NotFound();

            return View(wantedSlider);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Slider slider)
        {
            if (!ModelState.IsValid) return View();
            try
            {
                await _sliderService.UpdateAsync(slider);
            }
            catch (InvalidContentTypeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (InvalidImageSizeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (InvalidImageFileException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch(InvalidNullReferanceException ) { }
           
            

            return RedirectToAction("index");
        }

        public async Task<IActionResult>Delete(int id)
        {
            try
            {
                await _sliderService.DeleteAsync(id);
            }
            catch (InvalidNullReferanceException) { }
           
            return RedirectToAction("index");
        }
        //__________
        //[HttpPost]
        //public IActionResult Delete(Slider slider)
        //{
        //    Slider existSlider = _context.Sliders.FirstOrDefault(x => x.Id == slider.Id);
        //    string path = Path.Combine(_env.WebRootPath, "uploads/sliders", existSlider.ImageUrl);

        //   if(existSlider.ImageFile!=null)
        //    {
        //        if(System.IO.File.Exists(path))

        //        {
        //            System.IO.File.Delete(path);
        //        }
        //    }


        //    _context.Sliders.Remove(existSlider);
        //    _context.SaveChanges();

        //    return RedirectToAction("Index");
        //}
    }
}
