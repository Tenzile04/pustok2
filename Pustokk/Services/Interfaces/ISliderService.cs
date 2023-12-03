using Pustokk.Models;

namespace Pustokk.Services.Interfaces
{
    public interface ISliderService
    {
        Task CreatAsync(Slider slider);
        Task DeleteAsync(int id);
        Task<List<Slider>> GetAllAsync();
        Task<Slider> GetAsync(int id);
        Task UpdateAsync(Slider slider);


    }
}
