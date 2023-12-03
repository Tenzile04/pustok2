using Pustokk.Models;

namespace Pustokk.Services.Interfaces
{
    public interface IBookService
    {
        Task CreatAsync(Book book);
        Task DeleteAsync(int id);
        Task UpdateAsync(Book book);
        Task<List<Tag>> GetAllTagAsync();     
        Task<List<Genre>> GetAllGenreAsync();
        Task<List<Author>> GetAllAuthorAsync();

        Task<List<Book>> GetAllAsync();
        Task<Book> GetAsync(int id);
        
    }
}
