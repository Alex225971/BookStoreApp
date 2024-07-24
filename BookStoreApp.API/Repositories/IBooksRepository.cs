using BookStoreApp.API.Data;
using BookStoreApp.API.Models.Book;

namespace BookStoreApp.API.Repositories
{
    public interface IBooksRepository : IGenericRepository<Book>
    {
        Task<List<BookReadOnlyDto>> GetAllReadOnlyAsync();
        Task<BookDetailsDto> GetDetailsAsync(int id);
    }
}
