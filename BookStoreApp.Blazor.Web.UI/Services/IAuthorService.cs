using BookStoreApp.Blazor.Web.UI.Models;
using BookStoreApp.Blazor.Web.UI.Services.Base;

namespace BookStoreApp.Blazor.Web.UI.Services
{
    public interface IAuthorService
    {
        Task<Response<AuthorReadOnlyDtoVirtualizeResponse>> Get(QueryParams queryParams);
        Task<Response<List<AuthorReadOnlyDto>>> Get();
        Task<Response<AuthorDetailsDto>> GetAuthor(int id);
        Task<Response<AuthorUpdateDto>> GetAuthorUpdate(int id);
        Task<Response<int>> CreateAuthor(AuthorCreateDto author);
        Task<Response<int>> UpdateAuthor(int id, AuthorUpdateDto author);
        Task<Response<int>> Delete(int id);
    }
}