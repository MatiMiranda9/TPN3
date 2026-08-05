using CineApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CineApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class UploadController : ControllerBase
{
    private readonly ICloudinaryService _cloudinaryService;

    public UploadController(ICloudinaryService cloudinaryService)
    {
        _cloudinaryService = cloudinaryService;
    }

    [HttpPost("image")]
    public async Task<IActionResult> UploadImage(
        IFormFile file,
        [FromForm] string folder)
    {
        try
        {
            System.Diagnostics.Debug.WriteLine("Entró al UploadController");

            var folders = new[] { "movies", "articulos" };

            if (!folders.Contains(folder.ToLower()))
                return BadRequest("Carpeta inválida.");

            if (file == null)
                return BadRequest("No se recibió ningún archivo.");

            System.Diagnostics.Debug.WriteLine($"Nombre: {file?.FileName}");
            System.Diagnostics.Debug.WriteLine($"Tipo: {file?.ContentType}");
            System.Diagnostics.Debug.WriteLine($"Tamaño: {file?.Length}");

            var url = await _cloudinaryService.UploadImageAsync(file, folder);

            return Ok(new
            {
                Url = url
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.ToString());
        }
    }
        

    
}