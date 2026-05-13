using BCrypt.Net;
using CineApi.Data;
using DemoBlazorMovil.Shared.DTOs;
using DemoBlazorMovil.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CineApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly CineDBContext _context;

    private readonly IConfiguration _configuration;

    public UsersController(CineDBContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    // GET: api/users
    [Authorize(Roles = "Admin")]
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
            IdRol = u.IdRol,
            IsActive = u.IsActive
        }).ToList();
    }

    // GET: api/users/5
    [Authorize(Roles = "Admin")]
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
            IdRol = user.IdRol,
            IsActive = user.IsActive
        };
    }

    // POST: api/users
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<UserDto>> PostUser(UserCreateDto dto)
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

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            ImagePath = user.ImagePath,
            IdRol = user.IdRol,
            IsActive = user.IsActive
        });
    }

    // PUT: api/users/5
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(int id, UserDto dto)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound();

        user.Name = dto.Name;
        user.Email = dto.Email;
        user.ImagePath = dto.ImagePath;
        user.IdRol = dto.IdRol;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/users/5
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound();

        user.IsActive = false;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("activar/{id}")]
    public async Task<IActionResult> ActivarUser(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null) return NotFound();

        user.IsActive = true;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    //Registro

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDTO dto)
    {
        if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            return BadRequest("El email ya está registrado");

        var userRole = await _context.Roles
            .FirstOrDefaultAsync(r => r.Nombre == "User");

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            IdRol = 2,
            IsActive = true
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok();
    }

    // 🔹 LOGIN
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] UserLoginDto dto)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (user == null)
            return Unauthorized(new { message = "Email o contraseña incorrectos" });

        bool passwordOk = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);

        if (!passwordOk)
            return Unauthorized(new { message = "Email o contraseña incorrectos" });

        var token = GenerateJwtToken(user);

        return new LoginResponseDTO
        {
            Token = token,

            User = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                ImagePath = user.ImagePath,
                IdRol = user.IdRol,
                IsActive = user.IsActive
            }
        };
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSettings = _configuration.GetSection("Jwt");

        var claims = new[]
        {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Name),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role,
            user.IdRol == 1 ? "Admin" : "User")
    };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings["Key"]));

        var creds = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
