using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blazored.LocalStorage;
using DemoBlazorMovil.Shared.DTOs;
using System.Net.Http.Headers;

namespace DemoBlazorMovil.Services;

public class AuthService
{
    private readonly UserService _userService;
    private readonly ILocalStorageService _localStorage;

    private UserDto? _currentUser;

    public AuthService(
        UserService userService,
        ILocalStorageService localStorage)
    {
        _userService = userService;
        _localStorage = localStorage;
    }

    public UserDto? CurrentUser => _currentUser;

    public bool IsLoggedIn => _currentUser != null;

    public bool esAdmin => _currentUser?.IdRol == 1;

    public event Action? OnChange;

    public async Task<bool> LoginAsync(string email, string password)
    {
        var response = await _userService.ValidateLogin(email, password);

        if (response == null || string.IsNullOrWhiteSpace(response.Token))
            return false;

        await _localStorage.SetItemAsync("token", response.Token);

        ParseJwt(response.Token);

        NotifyStateChanged();

        return true;
    }

    public async Task InitializeAsync()
    {
        var token = await _localStorage.GetItemAsync<string>("token");

        if (!string.IsNullOrEmpty(token))
        {
            ParseJwt(token);
        }
    }

    private void ParseJwt(string token)
    {
        var handler = new JwtSecurityTokenHandler();

        var jwt = handler.ReadJwtToken(token);

        _currentUser = new UserDto
        {
            Id = int.Parse(jwt.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value),

            Name = jwt.Claims.First(x => x.Type == ClaimTypes.Name).Value,

            Email = jwt.Claims.First(x => x.Type == ClaimTypes.Email).Value,

            IdRol = jwt.Claims.First(x => x.Type == ClaimTypes.Role).Value == "Admin"
                ? 1
                : 2
        };
    }

    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync("token");

        _currentUser = null;

        NotifyStateChanged();
    }

    private void NotifyStateChanged()
    {
        OnChange?.Invoke();
    }
}