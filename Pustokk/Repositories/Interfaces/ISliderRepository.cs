using Pustokk.Models;

namespace Pustokk.Repositories.Interfaces
{
    public interface ISliderRepository
    {
        Task CreateAsync(Slider slider);
        Task<Slider> GetByIdAsync(int id);
        Task<List<Slider>> GetAllAsync();
        void Delete(Slider slider);
        Task<int> CommitAsync();

    }
}
