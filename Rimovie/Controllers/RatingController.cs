using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Rimovie.Entities;
using Rimovie.Excepciones;
using Rimovie.Models.Response;
using Rimovie.Repository;
using System;
using System.Security.Claims;
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
                throw new BadRequestException("La puntuación debe estar entre 1 y 5.");

            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
                throw new UnauthorizedException("No se encontró el ID del usuario en el token.");

            var userId = int.Parse(claim.Value);
            var success = await _ratingRepository.InsertOrUpdateAsync(userId, filmId, score);

            if (!success)
                throw new BadRequestException("No se pudo registrar la puntuación.");

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