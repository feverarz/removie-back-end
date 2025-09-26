using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rimovie.Entities;
using Rimovie.Models.Response;
using Rimovie.Repository;
using System;
using System.Threading.Tasks;

namespace Rimovie.Controllers
{
    [ApiController]
    [Route("rating")]
    public class RatingController : ControllerBase
    {
        private readonly RatingRepository _ratingRepository;

        public RatingController(RatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        // POST /rating/:filmId
        [HttpPost("{filmId}")]
        [Authorize]
        public async Task<IActionResult> RateFilm(int filmId, [FromBody] int score)
        {
            if (score < 1 || score > 5)
                return BadRequest("La puntuación debe estar entre 1 y 5.");

            var userId = int.Parse(User.FindFirst("id").Value);
            var success = await _ratingRepository.InsertOrUpdateAsync(userId, filmId, score);

            if (!success)
                return BadRequest("No se pudo registrar la puntuación.");

            return Ok();
        }

        // GET /rating/:filmId
        [HttpGet("{filmId}")]
        public async Task<IActionResult> GetAverage(int filmId)
        {
            var average = await _ratingRepository.GetAverageByFilmIdAsync(filmId);
            return Ok(new RatingResponseDto
            {
                FilmId = filmId,
                Average = Math.Round(average ?? 0, 2)
            });
        }
    }
}