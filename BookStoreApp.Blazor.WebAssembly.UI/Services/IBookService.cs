using BookStoreApp.Blazor.WebAssembly.UI.Services.Base;

namespace BookStoreApp.Blazor.WebAssembly.UI.Services
{
    public interface IBookService
    {
        Task<Response<List<BookReadOnlyDto>>> GetBooks();
        Task<Response<BookDetailsDto>> GetBook(int id);
        Task<Response<BookUpdateDto>> GetBookUpdate(int id);
        Task<Response<int>> CreateBook(BookCreateDto book);
        Task<Response<int>> UpdateBook(int id, BookUpdateDto book);
        Task<Response<int>> Delete(int id);
    }
}