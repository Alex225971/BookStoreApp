﻿using BookStoreApp.Blazor.Web.UI.Services.Base;

namespace BookStoreApp.Blazor.Web.UI.Services
{
    public interface IAuthorService
    {
        Task<Response<List<AuthorReadOnlyDto>>> GetAuthors();
        Task<Response<AuthorReadOnlyDto>> GetAuthor(int id);
        Task<Response<AuthorUpdateDto>> GetAuthorUpdate(int id);
        Task<Response<int>> CreateAuthor(AuthorCreateDto author);
        Task<Response<int>> UpdateAuthor(int id, AuthorUpdateDto author);
    }
}