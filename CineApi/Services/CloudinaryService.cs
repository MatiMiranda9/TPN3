using CineApi.Services.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace CineApi.Services;

public class CloudinaryService : ICloudinaryService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService(IConfiguration configuration)
    {
        var account = new Account(
            configuration["Cloudinary:CloudName"],
            configuration["Cloudinary:ApiKey"],
            configuration["Cloudinary:ApiSecret"]);

        _cloudinary = new Cloudinary(account);
    }

    public async Task<string> UploadImageAsync(IFormFile file, string folder)
    {
        const long maxSize = 5 * 1024 * 1024;

        if (file == null || file.Length == 0)
            throw new Exception("No se recibió ninguna imagen.");

        if (!file.ContentType.StartsWith("image/"))
            throw new Exception("El archivo debe ser una imagen.");

        if (file.Length > maxSize)
            throw new Exception("La imagen supera el tamaño máximo permitido (5 MB).");

        await using var stream = file.OpenReadStream();

        var uploadParams = new ImageUploadParams
        {
            PublicId = $"{Guid.NewGuid()}",
            File = new FileDescription(file.FileName, stream),
            Folder = folder
        };

        var result = await _cloudinary.UploadAsync(uploadParams);

        if (result.Error != null)
            throw new Exception(result.Error.Message);

        return result.SecureUrl.ToString();
    }
}