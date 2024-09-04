using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using LibraryManagementSystemApp.Models;

namespace LibraryManagementSystemApp.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;
        private readonly NavigationManager _navigationManager;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AuthService(HttpClient httpClient, IJSRuntime jsRuntime, NavigationManager navigationManager, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
            _navigationManager = navigationManager;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<bool> Login(LoginModel loginModel)
        {
            var loginData = new { Email = loginModel.Email, Password = loginModel.Password };
            var response = await _httpClient.PostAsJsonAsync("/api/v1/auth/login", loginData);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var token = JsonDocument.Parse(jsonResponse).RootElement.GetProperty("token").GetString();

                if (token != null)
                {
                    // Store the token in a cookie
                    await _jsRuntime.InvokeAsync<object>("WriteCookie.WriteCookie", "authToken", token, DateTime.Now.AddMinutes(1));

                    _navigationManager.NavigateTo("/");
                    return true;
                }
            }
            return false;
        }

        public async Task Logout()
        {
            try
            {
                // Clear the cookie
                await _jsRuntime.InvokeVoidAsync("DeleteCookie.DeleteCookie", "authToken");

                // Redirect to the login page
                _navigationManager.NavigateTo("/login");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Logout failed: {ex.Message}");
            }
        }

        public async Task<bool> IsAuthenticated()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (!user.Identity.IsAuthenticated)
            {
                _navigationManager.NavigateTo("/login");
                return false;
            }

            return true;
        }
    }
}
