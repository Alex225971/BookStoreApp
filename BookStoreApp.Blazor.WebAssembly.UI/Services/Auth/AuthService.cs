using Blazored.LocalStorage;
using BookStoreApp.Blazor.WebAssembly.UI.Providers;
using BookStoreApp.Blazor.WebAssembly.UI.Services.Base;
using Microsoft.AspNetCore.Components.Authorization;

namespace BookStoreApp.Blazor.WebAssembly.UI.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IClient _httpClient;
        private readonly ILocalStorageService _localStorageService;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AuthService(IClient httpClient, ILocalStorageService localStorageService, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public IClient HttpClient { get; }

        public async Task<bool> AuthenticateAsync(LoginUserDto loginModel)
        {
            var response = await _httpClient.LoginAsync(loginModel);

            await _localStorageService.SetItemAsync("accessToken", response.Token);

            await ((ApiAuthStateProvider)_authenticationStateProvider).LoggedIn();

            return true;
        }

        public async Task Logout()
        {
            await ((ApiAuthStateProvider)_authenticationStateProvider).LoggedOut();
        }
    }
}
