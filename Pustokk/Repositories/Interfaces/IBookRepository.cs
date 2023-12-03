using Pustokk.Models;

namespace Pustokk.Repositories.Interfaces
{
    public interface IBookRepository
    {
        Task CreateAsync(Book book);
        Task<Book> GetByIdAsync(int id);
        Task<List<Book>> GetAllAsync();
        void Delete(Book book);
        Task<int> CommitAsync();
        Task<List<Tag>> GetAllTagAsync();
        Task<List<Genre>> GetAllGenreAsync();
        Task<List<Author>> GetAllAuthorAsync();

        Task CreateBookTagAsync(BookTag booktag);
        Task CreateBookImageAsync(BookImage bookimage);

        Task<List<BookTag>> GetAllBookTagAsync();

        Task<List<BookImage>> GetAllBookImagesAsync();

    }
}
