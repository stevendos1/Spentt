using Spendnt.API.Application.Interfaces;
using Spendnt.Shared.DTOs.Categoria;
using Spendnt.Shared.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable

namespace Spendnt.API.Application.Services
{
    public class CategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public CategoriaService(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<IEnumerable<CategoriaDto>> GetCategoriasAsync()
        {
            var categorias = await _categoriaRepository.GetAllAsync();
            return categorias.Select(c => new CategoriaDto
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Descripcion = c.Descripcion
            });
        }

        public async Task<CategoriaDto?> GetCategoriaByIdAsync(int id)
        {
            var categoria = await _categoriaRepository.GetByIdAsync(id);
            if (categoria == null)
            {
                return null;
            }
            return new CategoriaDto
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion
            };
        }

        public async Task<CategoriaDto> CreateCategoriaAsync(CategoriaCreateDto categoriaDto)
        {
            var categoria = new Categoria
            {
                Nombre = categoriaDto.Nombre,
                Descripcion = categoriaDto.Descripcion
            };

            var nuevaCategoria = await _categoriaRepository.AddAsync(categoria);

            return new CategoriaDto
            {
                Id = nuevaCategoria.Id,
                Nombre = nuevaCategoria.Nombre,
                Descripcion = nuevaCategoria.Descripcion
            };
        }

        public async Task<bool> UpdateCategoriaAsync(int id, CategoriaCreateDto categoriaDto)
        {
            var categoria = await _categoriaRepository.GetByIdAsync(id);
            if (categoria == null)
            {
                return false;
            }

            categoria.Nombre = categoriaDto.Nombre;
            categoria.Descripcion = categoriaDto.Descripcion;

            await _categoriaRepository.UpdateAsync(categoria);
            return true;
        }

        public async Task<bool> DeleteCategoriaAsync(int id)
        {
            var existingCategoria = await _categoriaRepository.GetByIdAsync(id);
            if (existingCategoria == null)
            {
                return false;
            }
            await _categoriaRepository.DeleteAsync(id);
            return true;
        }
    }
}
