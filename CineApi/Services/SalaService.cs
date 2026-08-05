using CineApi.Data;
using CineApi.Services.Interfaces;
using DemoBlazorMovil.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CineApi.Services;

public class SalaService : ISalaService
{
    private readonly CineDBContext _context;

    public SalaService(CineDBContext context)
    {
        _context = context;
    }

    public async Task<List<SalaDTO>> GetAllAsync()
    {
        var salas = await _context.Salas.ToListAsync();

        return salas.Select(s => new SalaDTO
        {
            Id = s.Id,
            Nombre = s.Nombre,
            Capacidad = s.Capacidad
        }).ToList();
    }
}