using Blazored.LocalStorage;
using BookStoreApp.Blazor.Web.UI.Providers;
using BookStoreApp.Blazor.Web.UI.Services.Base;
using Microsoft.AspNetCore.Components.Authorization;

namespace BookStoreApp.Blazor.Web.UI.Services.Auth
{
    public class AuthService : BaseHttpService, IAuthService
    {
        private readonly IClient _httpClient;
        private readonly ILocalStorageService _localStorageService;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AuthService(IClient httpClient, ILocalStorageService localStorageService, AuthenticationStateProvider authenticationStateProvider) : base(httpClient, localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public IClient HttpClient { get; }

        public async Task<Response<AuthResponse>> AuthenticateAsync(LoginUserDto loginModel)
        {
            Response<AuthResponse> response;
            try
            {
                var result = await _httpClient.LoginAsync(loginModel);
                response = new Response<AuthResponse>
                {
                    Data = result,
                    Success = true
                };
                await _localStorageService.SetItemAsync("accessToken", result.Token);
                await ((ApiAuthStateProvider)_authenticationStateProvider).LoggedIn();
            }
            catch (ApiException e)
            {
                response = ConvertApiExceptions<AuthResponse>(e);
            }
            return response;
        }

        public async Task Logout()
        {
            await ((ApiAuthStateProvider)_authenticationStateProvider).LoggedOut();
        }
    }
}
