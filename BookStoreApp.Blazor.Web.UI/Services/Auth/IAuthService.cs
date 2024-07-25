using BookStoreApp.Blazor.Web.UI.Services.Base;

namespace BookStoreApp.Blazor.Web.UI.Services.Auth
{
    public interface IAuthService
    {
        Task<Response<AuthResponse>> AuthenticateAsync(LoginUserDto loginModel);
        public Task Logout();
    }
}
