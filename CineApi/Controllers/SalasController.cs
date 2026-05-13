using CineApi.Data;
using DemoBlazorMovil.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CineApi.Controllers;

[Route("api/[controller]")]
[ApiController]

public class SalasController : ControllerBase
    {
        private readonly CineDBContext _context;

        public SalasController(CineDBContext context)
        {
            _context = context;
        }

    [HttpGet]
        public async Task<ActionResult<IEnumerable<SalaDTO>>> GetSalas()
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

