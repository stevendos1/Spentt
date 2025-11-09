// Spendnt.API/Controllers/CategoriasController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spendnt.API.Application.Services;
using Spendnt.Shared.DTOs.Categoria;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable

namespace Spendnt.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly CategoriaService _categoriaService;

        public CategoriasController(CategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDto>>> Get()
        {
            var categorias = await _categoriaService.GetCategoriasAsync();
            return Ok(categorias);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoriaDto>> Get(int id)
        {
            var categoria = await _categoriaService.GetCategoriaByIdAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return Ok(categoria);
        }

        [HttpPost]
        public async Task<ActionResult<CategoriaDto>> Post(CategoriaCreateDto categoriaDto)
        {
            var nuevaCategoria = await _categoriaService.CreateCategoriaAsync(categoriaDto);
            return CreatedAtAction(nameof(Get), new { id = nuevaCategoria.Id }, nuevaCategoria);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, CategoriaCreateDto categoriaDto)
        {
            var result = await _categoriaService.UpdateCategoriaAsync(id, categoriaDto);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoriaService.DeleteCategoriaAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}