using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rimovie.Entities;
using Rimovie.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rimovie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmsController : ControllerBase
    {
        private readonly FilmRepository _filmRepository;
        public FilmsController(FilmRepository repository)
        {
            _filmRepository = repository;
        }

        [HttpGet("v1/all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var response = await _filmRepository.GetAllAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpGet("v1/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var response = await _filmRepository.GetByIdAsync(id);
                if (response == null)
                    return NotFound($"Film with ID {id} not found.");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }
        [HttpPost("v1")]
        public async Task<IActionResult> Create([FromBody] Film film)
        {
            try
            {
                if (film == null)
                    return BadRequest("Film data is required.");
                var newFilmId = await _filmRepository.InsertAsync(film);
                return CreatedAtAction(nameof(GetById), new { id = newFilmId }, new { filmId = newFilmId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpPost("v1/insertList")]
        public async Task<IActionResult> CreateList([FromBody] List<Film> films)
        {
            try
            {
                if (films == null || !films.Any())
                    return BadRequest("Film list is required.");
                var createdFilmIds = new List<int>();
                foreach (var film in films)
                {
                    var newFilmId = await _filmRepository.InsertAsync(film);
                    createdFilmIds.Add(newFilmId);
                }
                return Ok(new { createdFilmIds });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }
    }
}
