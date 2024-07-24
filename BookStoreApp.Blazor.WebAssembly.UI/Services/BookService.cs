using AutoMapper;
using Blazored.LocalStorage;
using BookStoreApp.Blazor.WebAssembly.UI.Services.Base;

namespace BookStoreApp.Blazor.WebAssembly.UI.Services
{
    public class BookService : BaseHttpService, IBookService
    {
        private readonly IClient _client;
        private readonly IMapper _mapper;

        public BookService(IClient client, ILocalStorageService localStorageService, IMapper mapper) : base(client, localStorageService)
        {
            _client = client;
            _mapper = mapper;
        }

        public async Task<Response<int>> CreateBook(BookCreateDto book)
        {
            Response<int> response = new();
            try
            {
                await GetBearerToken();
                await _client.BooksPOSTAsync(book);
            }
            catch (ApiException e)
            {

                response = ConvertApiExceptions<int>(e);
            }
            return response;
        }

        public async Task<Response<int>> Delete(int id)
        {
            Response<int> response = new();

            try
            {
                await GetBearerToken();
                await _client.BooksDELETEAsync(id);
            }
            catch (ApiException e)
            {

                response = ConvertApiExceptions<int>(e);
            }
            return response;
        }

        public async Task<Response<BookDetailsDto>> GetBook(int id)
        {
            Response<BookDetailsDto> response;

            try
            {
                await GetBearerToken();
                var data = await _client.BooksGETAsync(id);
                response = new Response<BookDetailsDto>
                {
                    Data = data,
                    Success = true
                };
            }
            catch (ApiException e)
            {

                response = ConvertApiExceptions<BookDetailsDto>(e);
            }

            return response;
        }

        public async Task<Response<List<BookReadOnlyDto>>> GetBooks()
        {
            Response<List<BookReadOnlyDto>> response;

            try
            {
                await GetBearerToken();
                var data = await _client.BooksAllAsync();
                response = new Response<List<BookReadOnlyDto>>
                {
                    Data = data.ToList(),
                    Success = true
                };
            }
            catch (ApiException e)
            {

                response = ConvertApiExceptions<List<BookReadOnlyDto>>(e);
            }

            return response;
        }

        public async Task<Response<BookUpdateDto>> GetBookUpdate(int id)
        {
            Response<BookUpdateDto> response;

            try
            {
                await GetBearerToken();
                var data = await _client.BooksGETAsync(id);
                response = new Response<BookUpdateDto>
                {
                    Data = _mapper.Map<BookUpdateDto>(data),
                    Success = true
                };
            }
            catch (ApiException e)
            {

                response = ConvertApiExceptions<BookUpdateDto>(e);
            }

            return response;
        }

        public async Task<Response<int>> UpdateBook(int id, BookUpdateDto book)
        {
            Response<int> response = new();
            try
            {
                await GetBearerToken();
                await _client.BooksPUTAsync(id, book);
            }
            catch (ApiException e)
            {

                response = ConvertApiExceptions<int>(e);
            }
            return response;
        }
    }
}
