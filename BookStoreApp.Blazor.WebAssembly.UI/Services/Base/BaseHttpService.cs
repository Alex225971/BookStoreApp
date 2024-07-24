using Blazored.LocalStorage;
using System.Net.Http.Headers;

namespace BookStoreApp.Blazor.WebAssembly.UI.Services.Base
{
    public class BaseHttpService
    {
        private readonly IClient _client;
        private readonly ILocalStorageService _localStorageService;

        public BaseHttpService(IClient client, ILocalStorageService localStorageService)
        {
            _client = client;
            _localStorageService = localStorageService;
        }

        protected Response<Guid> ConvertApiExceptions<Guid>(ApiException exception)
        {
            if (exception.StatusCode == 400)
            {
                return new Response<Guid>() { Message = "Validation error", ValidationErrors = exception.Response, Success = false };
            }

            if (exception.StatusCode == 404)
            {
                return new Response<Guid>() { Message = "Item not found", Success = false };
            }

            if(exception.StatusCode >= 200 && exception.StatusCode <= 299)
            {
                return new Response<Guid>() { Message = "Operation successful", Success = true };
            }
            return new Response<Guid>() { Message = "Something went wrong, please try again", Success = false };
        }

        protected async Task GetBearerToken()
        {
            var token = await _localStorageService.GetItemAsync<string>("accessToken");
            if (token != null)
            {
                _client.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            }
        }
    }
}
