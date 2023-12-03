using Pustokk.CustomExceptions.SliderException;
using Pustokk.Extencions;
using Pustokk.Models;
using Pustokk.Repositories.Interfaces;
using Pustokk.Services.Interfaces;

namespace Pustokk.Services.Implementations
{
    public class SliderService : ISliderService
    {
        private IWebHostEnvironment _env;
        private readonly ISliderRepository _sliderRepository;

        public SliderService(IWebHostEnvironment env, ISliderRepository sliderRepository)
        {
            _env = env;
            _sliderRepository = sliderRepository;
        }
        public async Task CreatAsync(Slider slider)
        {

            if (slider.ImageFile != null)
            {

                string fileName = slider.ImageFile.FileName;
                if (slider.ImageFile.ContentType != "image/jpeg" && slider.ImageFile.ContentType != "image/png")
                {
                    throw new InvalidContentTypeException ("ImageFile", "ContentType must be jpeg or png");
                }

                if (slider.ImageFile.Length > 2097152)
                {
                    throw new InvalidImageSizeException("ImageFile", "File size must be lower than 1mb");
                }


                slider.ImageUrl = Helper.SaveFile(_env.WebRootPath, "uploads/sliders", slider.ImageFile);
            }
            else
            {
                throw new InvalidImageFileException("ImageFile", "Required!");
            }
            await _sliderRepository.CreateAsync(slider);
            await _sliderRepository.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var slider = await _sliderRepository.GetByIdAsync(id);
            if (slider == null) throw new InvalidNullReferanceException();
            _sliderRepository.Delete(slider);
            _sliderRepository.CommitAsync();

        }

        public async Task<List<Slider>> GetAllAsync()
        {
            return await _sliderRepository.GetAllAsync();
        }

        public async Task<Slider> GetAsync(int id)
        {
            return await _sliderRepository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(Slider slider)
        {
            Slider existSlider = await _sliderRepository.GetByIdAsync(slider.Id);
            if (existSlider == null) throw new InvalidNullReferanceException();

            if (slider.ImageFile != null)
            {

                if (slider.ImageFile.ContentType != "image/jpeg" && slider.ImageFile.ContentType != "image/png")
                {
                    throw new InvalidContentTypeException ("ImageFile", "ContentType must be jpeg or png");
                }

                if (slider.ImageFile.Length > 2097152)
                {
                    throw new InvalidImageSizeException ("ImageFile", "File size must be lower than 1mb");

                }
              
                string deletepath = Path.Combine(_env.WebRootPath, "uploads/sliders", existSlider.ImageUrl);
                if (System.IO.File.Exists(deletepath))
                {
                    System.IO.File.Delete(deletepath);
                }

                existSlider.ImageUrl = Helper.SaveFile(_env.WebRootPath, "uploads/sliders", slider.ImageFile);
            }
            existSlider.Title = slider.Title;
            existSlider.Description = slider.Description;
            existSlider.RedirectUrl = slider.RedirectUrl;
            existSlider.RedirecText = slider.RedirecText;

            await _sliderRepository.CommitAsync();
        }
    }
}
