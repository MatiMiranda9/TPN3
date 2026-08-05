using CineApi.Services.Interfaces;
using DemoBlazorMovil.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CineApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    private readonly IAuthService _authService;

    public UsersController(
        IUserService userService,
        IAuthService authService)
    {
        _userService = userService;
        _authService = authService;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        return Ok(await _userService.GetAllAsync());
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _userService.GetByIdAsync(id);

        if (user == null)
            return NotFound();

        return Ok(user);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> PostUser(UserCreateDto dto)
    {
        var user = await _userService.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetUser),
            new { id = user.Id },
            user);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(int id, UserDto dto)
    {
        var ok = await _userService.UpdateAsync(id, dto);

        if (!ok)
            return NotFound();

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var ok = await _userService.DeleteAsync(id);

        if (!ok)
            return NotFound();

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("activar/{id}")]
    public async Task<IActionResult> ActivarUser(int id)
    {
        var ok = await _userService.ActivateAsync(id);

        if (!ok)
            return NotFound();

        return NoContent();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDTO dto)
    {
        var ok = await _authService.RegisterAsync(dto);

        if (!ok)
            return BadRequest("El email ya está registrado");

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginDto dto)
    {
        var response = await _authService.LoginAsync(dto);

        if (response == null)
            return Unauthorized(
                new
                {
                    message = "Email o contraseña incorrectos"
                });

        return Ok(response);
    }
}