using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rimovie.Entities;
using Rimovie.Excepciones;
using Rimovie.Mappers;
using Rimovie.Models.Request;
using Rimovie.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rimovie.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilmController : ControllerBase
    {
        private readonly FilmRepository _filmRepository;

        public FilmController(FilmRepository filmRepository)
        {
            _filmRepository = filmRepository;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateFilm([FromBody] FilmCreateDto dto)
        {

            if (!ModelState.IsValid)
                throw new BadRequestException("El modelo enviado no es válido");

            var film = FilmMapper.ToEntity(dto);
            var filmId = await _filmRepository.InsertAsync(film);
            return Ok(new { FilmId = filmId });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFilm(int id)
        {
            var film = await _filmRepository.GetByIdAsync(id);
            if (film is null)
                throw new NotFoundException($"No se encontró la película con ID {id}");

            var dto = FilmMapper.ToResponseDto(film);
            return Ok(dto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var films = await _filmRepository.GetAllAsync();
            var dtos = films.Select(FilmMapper.ToResponseDto);
            return Ok(dtos);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateFilm(int id, [FromBody] FilmCreateDto dto)
        {
                if (!ModelState.IsValid)
                    throw new BadRequestException("El modelo enviado no es válido");

                var film = FilmMapper.ToEntity(dto);
                film.FilmId = id;

                var success = await _filmRepository.UpdateAsync(film);
                if (!success)
                    throw new NotFoundException($"No se encontró la película con ID {id} para actualizar");

                return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteFilm(int id)
        {
                var success = await _filmRepository.DeleteAsync(id);
                if (!success)
                    throw new NotFoundException($"No se encontró la película con ID {id} para eliminar");

                return NoContent();
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportFilms([FromBody] List<FilmImportDto> films)
        {
                foreach (var dto in films)
                {
                    // Insertar género si no existe
                    var genderId = await _filmRepository.GetOrCreateAsync(dto.Gender.Name);

                    var film = new Film
                    {
                        Title = dto.Title,
                        Description = dto.Synopsis,
                        ReleaseYear = dto.Year,
                        PosterUrl = dto.Poster_Url,
                        TrailerUrl = "", // lo podés dejar vacío por ahora
                        Genres = new List<string> { dto.Gender.Name } // si usás strings por ahora
                    };

                    await _filmRepository.InsertAsync(film);
                }

                return Ok(new { inserted = films.Count });
        }
    }
}