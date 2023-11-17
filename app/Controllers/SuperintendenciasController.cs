using app.Entidades;
using app.Services;
using app.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace app.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SuperintendenciasController
{
    private ISuperintendenciaService _superintendenciaService;

    public SuperintendenciasController(ISuperintendenciaService superintendenciaService)
    {
        _superintendenciaService = superintendenciaService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Superintendencia>> GetSuperItendencia(int id)
    { 
        return await _superintendenciaService.ObterPorIdAsync(id);
    }
}
