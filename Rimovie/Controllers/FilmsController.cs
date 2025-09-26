using Rimovie.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rimovie.Models.Request;
using Rimovie.Repository;
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
                return BadRequest(ModelState);

            var film = FilmMapper.ToEntity(dto);
            var filmId = await _filmRepository.InsertAsync(film);
            return Ok(new { FilmId = filmId });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFilm(int id)
        {
            var film = await _filmRepository.GetByIdAsync(id);
            if (film is null)
                return NotFound();

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
                return BadRequest(ModelState);

            var film = FilmMapper.ToEntity(dto);
            film.FilmId = id;

            var success = await _filmRepository.UpdateAsync(film);
            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteFilm(int id)
        {
            var success = await _filmRepository.DeleteAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}