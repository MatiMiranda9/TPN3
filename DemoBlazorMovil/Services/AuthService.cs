using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoBlazorMovil.Models;

namespace DemoBlazorMovil.Services;

public class AuthService
{
    private User? _currentUser;

    public User? CurrentUser => _currentUser;
    public bool IsAdmin => _currentUser?.IsAdmin ?? false;
    public bool IsLoggedIn => _currentUser != null;

    public event Action? OnChange;

    public bool Login(string email, string password, UserService userService)
    {
        var user = userService.ValidateLogin(email, password);
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

