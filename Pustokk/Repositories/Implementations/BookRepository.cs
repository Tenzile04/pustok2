using Microsoft.EntityFrameworkCore;
using Pustokk.DAL;
using Pustokk.Models;
using Pustokk.Repositories.Interfaces;

namespace Pustokk.Repositories.Implementations
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _context;
        public BookRepository(AppDbContext context)
        {
            _context = context;  
        }
        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task CreateAsync(Book book)
        {
            await _context.Books.AddAsync(book);
        }

       
        public  void Delete(Book book)
        {
             _context.Books.Remove(book);
        }

        public async Task<List<Book>> GetAllAsync()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<List<Author>> GetAllAuthorAsync()
        {
            return await _context.Authors.ToListAsync();
        }


        public async Task<List<Genre>> GetAllGenreAsync()
        {
            return await _context.Genres.ToListAsync();
        }

        public async Task<List<Tag>> GetAllTagAsync()
        {
            return await _context.Tags.ToListAsync();
        }

        public async Task<Book> GetByIdAsync(int id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task<List<BookImage>> GetAllBookImagesAsync()
        {
            return await _context.BookImages.ToListAsync();
        }

        public async Task<List<BookTag>> GetAllBookTagAsync()
        {
            return await _context.BookTags.ToListAsync();
        }
        public async Task CreateBookImageAsync(BookImage bookimage)
        {
            await _context.BookImages.AddAsync(bookimage);
        }

        public async Task CreateBookTagAsync(BookTag booktag)
        {
            await _context.BookTags.AddAsync(booktag);
        }
    }
}
