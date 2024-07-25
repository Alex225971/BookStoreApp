using AutoMapper;
using Blazored.LocalStorage;
using BookStoreApp.Blazor.Web.UI.Models;
using BookStoreApp.Blazor.Web.UI.Services.Base;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace BookStoreApp.Blazor.Web.UI.Services
{
    public class AuthorService : BaseHttpService, IAuthorService
    {
        private readonly IClient _client;
        private readonly IMapper _mapper;

        public AuthorService(IClient client, ILocalStorageService localStorageService, IMapper mapper) : base(client, localStorageService)
        {
            _client = client;
            _mapper = mapper;
        }

        public async Task<Response<int>> CreateAuthor(AuthorCreateDto author)
        {
            Response<int> response = new();
            try
            {
                await GetBearerToken();
                await _client.AuthorsPOSTAsync(author);
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
                await _client.AuthorsDELETEAsync(id);
            }
            catch (ApiException e)
            {

                response = ConvertApiExceptions<int>(e);
            }
            return response;
        }

        public async Task<Response<AuthorDetailsDto>> GetAuthor(int id)
        {
            Response<AuthorDetailsDto> response;

            try
            {
                await GetBearerToken();
                var data = await _client.AuthorsGET2Async(id);
                response = new Response<AuthorDetailsDto>
                {
                    Data = data,
                    Success = true
                };
            }
            catch (ApiException e)
            {

                response = ConvertApiExceptions<AuthorDetailsDto>(e);
            }

            return response;
        }

        public async Task<Response<AuthorReadOnlyDtoVirtualizeResponse>> Get(QueryParams queryParams)
        {
            Response<AuthorReadOnlyDtoVirtualizeResponse> response;

            try
            {
                await GetBearerToken();
                var data = await _client.AuthorsGETAsync(queryParams.StartIndex, queryParams.PageSize);
                response = new Response<AuthorReadOnlyDtoVirtualizeResponse>
                {
                    Data = data,
                    Success = true
                };
            }
            catch (ApiException e)
            {

                response = ConvertApiExceptions<AuthorReadOnlyDtoVirtualizeResponse>(e);
            }

            return response;
        }

        public async Task<Response<AuthorUpdateDto>> GetAuthorUpdate(int id)
        {
            Response<AuthorUpdateDto> response;

            try
            {
                await GetBearerToken();
                var data = await _client.AuthorsGET2Async(id);
                response = new Response<AuthorUpdateDto>
                {
                    Data = _mapper.Map<AuthorUpdateDto>(data),
                    Success = true
                };
            }
            catch (ApiException e)
            {

                response = ConvertApiExceptions<AuthorUpdateDto>(e);
            }

            return response;
        }

        public async Task<Response<int>> UpdateAuthor(int id, AuthorUpdateDto author)
        {
            Response<int> response = new();
            try
            {
                await GetBearerToken();
                await _client.AuthorsPUTAsync(id, author);
            }
            catch (ApiException e)
            {

                response = ConvertApiExceptions<int>(e);
            }
            return response;
        }

        public async Task<Response<List<AuthorReadOnlyDto>>> Get()
        {
            Response<List<AuthorReadOnlyDto>> response;

            try
            {
                await GetBearerToken();
                var data = await _client.GetAllAsync();
                response = new Response<List<AuthorReadOnlyDto>>
                {
                    Data = data.ToList(),
                    Success = true
                };
            }
            catch (ApiException e)
            {
                response = ConvertApiExceptions<List<AuthorReadOnlyDto>>(e);
            }
            return response;
        }
    }
}
