using CineApi.Data;
using CineApi.Services.Interfaces;
using DemoBlazorMovil.Shared.DTOs;
using DemoBlazorMovil.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace CineApi.Services;

public class UserService : IUserService
{
    private readonly CineDBContext _context;

    public UserService(CineDBContext context)
    {
        _context = context;
    }

    public async Task<List<UserDto>> GetAllAsync()
    {
        return await _context.Users
            .Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                ImagePath = u.ImagePath,
                IdRol = u.IdRol,
                IsActive = u.IsActive
            })
            .ToListAsync();
    }

    public async Task<UserDto?> GetByIdAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
            return null;

        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            ImagePath = user.ImagePath,
            IdRol = user.IdRol,
            IsActive = user.IsActive
        };
    }

    public async Task<UserDto> CreateAsync(UserCreateDto dto)
    {
        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            ImagePath = dto.ImagePath,
            IdRol = dto.IdRol,
            IsActive = true
        };

        _context.Users.Add(user);

        await _context.SaveChangesAsync();

        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            ImagePath = user.ImagePath,
            IdRol = user.IdRol,
            IsActive = user.IsActive
        };
    }

    public async Task<bool> UpdateAsync(int id, UserDto dto)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
            return false;

        user.Name = dto.Name;
        user.Email = dto.Email;
        user.ImagePath = dto.ImagePath;
        user.IdRol = dto.IdRol;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
            return false;

        user.IsActive = false;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ActivateAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
            return false;

        user.IsActive = true;

        await _context.SaveChangesAsync();

        return true;
    }
}