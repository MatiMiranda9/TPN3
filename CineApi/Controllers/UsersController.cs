using DemoBlazorMovil.Shared.DTOs;
using DemoBlazorMovil.Shared.Models;
using CineApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CineApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly CineDbContext _context;

    public UsersController(CineDbContext context)
    {
        _context = context;
    }

    // GET: api/users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        var users = await _context.Users.ToListAsync();

        return users.Select(u => new UserDto
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
            ImagePath = u.ImagePath,
            IsAdmin = u.IsAdmin
        }).ToList();
    }

    // GET: api/users/5
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null) return NotFound();

        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            ImagePath = user.ImagePath,
            IsAdmin = user.IsAdmin
        };
    }

    // POST: api/users
    [HttpPost]
    public async Task<ActionResult<UserDto>> PostUser(UserCreateDto dto)
    {
        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            Password = dto.Password,
            ImagePath = dto.ImagePath,
            IsAdmin = dto.IsAdmin
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            ImagePath = user.ImagePath,
            IsAdmin = user.IsAdmin
        });
    }

    // PUT: api/users/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(int id, UserCreateDto dto)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound();

        user.Name = dto.Name;
        user.Email = dto.Email;
        user.ImagePath = dto.ImagePath;
        user.IsAdmin = dto.IsAdmin;

        // 🔹 Solo actualizar contraseña si se envía
        if (!string.IsNullOrWhiteSpace(dto.Password))
        {
            user.Password = dto.Password;
        }

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/users/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound();

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // 🔹 LOGIN
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login([FromBody] UserLoginDto dto)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == dto.Email && u.Password == dto.Password);

        if (user == null)
            return Unauthorized(new { message = "Email o contraseña incorrectos" });

        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            ImagePath = user.ImagePath,
            IsAdmin = user.IsAdmin
        };
    }
}
