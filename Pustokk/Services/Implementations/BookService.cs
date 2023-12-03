using Microsoft.EntityFrameworkCore;
using Pustokk.Extencions;
using Pustokk.Models;
using Pustokk.Repositories.Interfaces;
using Pustokk.Services.Interfaces;

namespace Pustokk.Services.Implementations
{
    public class BookService : IBookService
    {
        private IWebHostEnvironment _env;
        private readonly IBookRepository _bookRepository;

        public BookService(IWebHostEnvironment env, IBookRepository bookRepository)
        {
            _env = env;
            _bookRepository = bookRepository;
        }

        public async Task CreatAsync(Book book)
        {

            List<Author> Authors = await _bookRepository.GetAllAuthorAsync();
            List<Genre> Genres = await _bookRepository.GetAllGenreAsync();
            List<Tag> Tags = await _bookRepository.GetAllTagAsync();
            List<BookTag> BookTags = await _bookRepository.GetAllBookTagAsync();
            List<BookImage> BookImages = await _bookRepository.GetAllBookImagesAsync();
            if (!Genres.Any(x => x.Id == book.GenreId))
            {
                throw new Exception(); /*("GenreId", "Genre Not found");*/

            }
            if (!Authors.Any(x => x.Id == book.AuthorId))
            {
                throw new Exception(); /*("AuthorId", "Author Not found");*/

            }

            bool check = false;
            if (book.TagIds != null)
            {
                foreach (var tagId in book.TagIds)
                {
                    if (!Tags.Any(x => x.Id == tagId))
                    {
                        check = true;
                        break;
                    }
                }
            }
            if (check)
            {
                throw new Exception(); /*("TagId", "Tag not found");*/

            }
            else
            {
                if (book.TagIds != null)
                {
                    foreach (var tagId in book.TagIds)
                    {
                        BookTag booktag = new BookTag
                        {
                            Book = book,
                            TagId = tagId,

                        };
                        await _bookRepository.CreateBookTagAsync(booktag);
                    }
                }
            }

            if (book.BookPosterImageFile != null)
            {
                if (book.BookPosterImageFile.ContentType != "image/jpeg" && book.BookPosterImageFile.ContentType != "image/png")
                {
                    throw new Exception();   //("BookPosterImageFile", "can only upload .jpeg or .png");

                }

                if (book.BookPosterImageFile.Length > 1048576)
                {
                    throw new Exception();  /*("BookPosterImageFile", "File size must be lower than 1mb");*/

                }
                BookImage bookImage = new BookImage
                {
                    Book = book,
                    ImageUrl = Helper.SaveFile(_env.WebRootPath, "uploads/books", book.BookPosterImageFile),
                    IsPoster = true
                };
                await _bookRepository.CreateBookImageAsync(bookImage);
            }

            if (book.BookHoverImageFile != null)
            {
                if (book.BookHoverImageFile.ContentType != "image/jpeg" && book.BookHoverImageFile.ContentType != "image/png")
                {
                    throw new Exception(); /*("BookHoverImageFile", "can only upload .jpeg or .png");*/

                }

                if (book.BookHoverImageFile.Length > 1048576)
                {
                    throw new Exception();/* ("BookHoverImageFile", "File size must be lower than 1mb");*/

                }
                BookImage bookImage = new BookImage
                {
                    Book = book,
                    ImageUrl = Helper.SaveFile(_env.WebRootPath, "uploads/books", book.BookHoverImageFile),
                    IsPoster = false
                };
                await _bookRepository.CreateBookImageAsync(bookImage);
            }
            if (book.ImageFiles != null)
            {
                foreach (var imageFile in book.ImageFiles)
                {
                    if (imageFile.ContentType != "image/jpeg" && imageFile.ContentType != "image/png")
                    {
                        throw new Exception(); /*("ImageFiles", "can only upload .jpeg or .png");*/

                    }

                    if (imageFile.Length > 1048576)
                    {
                        throw new Exception();/*("ImageFiles", "File size must be lower than 1mb");*/

                    }
                    BookImage bookImage = new BookImage
                    {
                        Book = book,
                        ImageUrl = Helper.SaveFile(_env.WebRootPath, "uploads/books", imageFile),
                        IsPoster = null
                    };
                    await _bookRepository.CreateBookImageAsync(bookImage);
                }
            }


            await _bookRepository.CreateAsync(book);
            await _bookRepository.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Book book = await _bookRepository.GetByIdAsync(id);
            if (book == null) throw new NullReferenceException();

            _bookRepository.Delete(book);
            _bookRepository.CommitAsync();
        }

        public async Task UpdateAsync(Book book)
        {
            List<Book> Books = await _bookRepository.GetAllAsync();
            List<Author> Authors = await _bookRepository.GetAllAuthorAsync();
            List<Genre> Genres = await _bookRepository.GetAllGenreAsync();
            List<Tag> Tags = await _bookRepository.GetAllTagAsync();
            List<BookTag> BookTags = await _bookRepository.GetAllBookTagAsync();
            List<BookImage> BookImages = await _bookRepository.GetAllBookImagesAsync();

            //Book existbook =await _bookRepository.Include(x => x.BookTags).FirstOrDefault(x => x.Id == book.Id);
            book existbook = await _bookrepository.getbyıdasync(book.Id);
            if (existbook == null) throw new NullReferenceException();
            if (!Genres.Any(x => x.Id == book.GenreId))
            {
                throw new Exception(); /*("GenreId", "Genre Not found");*/

            }
            if (!Authors.Any(x => x.Id == book.AuthorId))
            {
                throw new Exception(); /*("AuthorId", "Author Not found");*/

            }

            existbook.BookTags.RemoveAll(bt => !book.TagIds.Contains(bt.TagId));

            foreach (var tagId in book.TagIds.Where(t => !existbook.BookTags.Any(bt => bt.TagId == t)))
            {
                BookTag bookTag = new BookTag
                {
                    TagId = tagId
                };
                existbook.BookTags.Add(bookTag);
            }


            if (book.BookPosterImageFile != null)
            {
                if (book.BookPosterImageFile.ContentType != "image/jpeg" && book.BookPosterImageFile.ContentType != "image/png")
                {
                    throw new Exception();   //("BookPosterImageFile", "can only upload .jpeg or .png");

                }

                if (book.BookPosterImageFile.Length > 1048576)
                {
                    throw new Exception();/*("ImageFiles", "File size must be lower than 1mb");*/

                }
                existbook.BookImages.Remove(existbook.BookImages.FirstOrDefault(x => x.IsPoster == true));

                BookImage bookImage = new BookImage
                {
                    Book = book,
                    ImageUrl = Helper.SaveFile(_env.WebRootPath, "uploads/books", book.BookPosterImageFile),
                    IsPoster = true
                };
                existbook.BookImages.Add(bookImage);
            }

            if (book.BookHoverImageFile != null)
            {
                if (book.BookHoverImageFile.ContentType != "image/jpeg" && book.BookHoverImageFile.ContentType != "image/png")
                {
                    throw new Exception();   //("BookPosterImageFile", "can only upload .jpeg or .png");
                }

                if (book.BookHoverImageFile.Length > 1048576)
                {
                    throw new Exception();/*("ImageFiles", "File size must be lower than 1mb");*/
                }
                existbook.BookImages.Remove(existbook.BookImages.FirstOrDefault(x => x.IsPoster == false));

                BookImage bookImage = new BookImage
                {
                    Book = book,
                    ImageUrl = Helper.SaveFile(_env.WebRootPath, "uploads/books", book.BookHoverImageFile),
                    IsPoster = false
                };
                existbook.BookImages.Add(bookImage);
            }

            if (book.ImageFiles != null)
            {
                if (existbook.BookImages is not null)
                {
                    existbook.BookImages.RemoveAll(bi => !book.BookImageIds.Contains(bi.Id) && bi.IsPoster == null);
                }
                foreach (var imageFile in book.ImageFiles)
                {
                    if (imageFile.ContentType != "image/jpeg" && imageFile.ContentType != "image/png")
                    {
                        throw new Exception();/*("ImageFiles", "File size must be lower than 1mb");*/
                    }

                    if (imageFile.Length > 1048576)
                    {
                        throw new Exception();   //("BookPosterImageFile", "can only upload .jpeg or .png");
                    }
                    BookImage bookImage = new BookImage
                    {
                        Book = book,
                        ImageUrl = Helper.SaveFile(_env.WebRootPath, "uploads/books", imageFile),
                        IsPoster = null
                    };
                    existbook.BookImages.Add(bookImage);
                }
            }

            existbook.Name = book.Name;
            existbook.Description = book.Description;
            existbook.SalePrice = book.SalePrice;
            existbook.CostPrice = book.CostPrice;
            existbook.DiscountPercent = book.DiscountPercent;
            existbook.IsAvailable = book.IsAvailable;
            existbook.Tax = book.Tax;
            existbook.Code = book.Code;
            existbook.AuthorId = book.AuthorId;
            existbook.GenreId = book.GenreId;
            await _bookRepository.CommitAsync();

        }

        public async Task<List<Book>> GetAllAsync()
        {
            return await _bookRepository.GetAllAsync();
        }

        public async Task<Book> GetAsync(int id)
        {
            return await _bookRepository.GetByIdAsync(id);
        }

        public async Task<List<Tag>> GetAllTagAsync()
        {
            return await _bookRepository.GetAllTagAsync();
        }

        public async Task<List<Genre>> GetAllGenreAsync()
        {
            return await _bookRepository.GetAllGenreAsync();
        }

        public async Task<List<Author>> GetAllAuthorAsync()
        {
            return await _bookRepository.GetAllAuthorAsync();
        }

    }
}
