using DemoBlazorMovil.Shared.DTOs;

namespace DemoBlazorMovil.Services;

public class AuthService
{
    private readonly UserService _userService;
    private UserDto? _currentUser;

    public AuthService(UserService userService)
    {
        _userService = userService;
    }

    public UserDto? CurrentUser => _currentUser;
    public bool IsAdmin => _currentUser?.IsAdmin ?? false;
    public bool IsLoggedIn => _currentUser != null;

    public event Action? OnChange;

    public async Task<bool> LoginAsync(string email, string password)
    {
        var user = await _userService.ValidateLogin(email, password);
        if (user == null) return false;

        _currentUser = user;
        NotifyStateChanged();
        return true;
    }

    public void Logout()
    {
        _currentUser = null;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
