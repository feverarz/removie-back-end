using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rimovie.Models;
using Rimovie.Repository;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Rimovie.Controllers
{
    [ApiController]
    [Route("wishlist")]
    public class WishListController : ControllerBase
    {
        private readonly WishListRepository _wishListRepository;
        private readonly WishListFilmRepository _wishListFilmRepository;

        public WishListController(
            WishListRepository wishListRepository,
            WishListFilmRepository wishListFilmRepository)
        {
            _wishListRepository = wishListRepository;
            _wishListFilmRepository = wishListFilmRepository;
        }

        // GET /wishlist
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMyWishlists()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
                return Unauthorized("No se encontró el ID del usuario en el token.");

            var userId = int.Parse(claim.Value);
            var wishlists = await _wishListRepository.GetWithFilmsByUserAsync(userId);
            return Ok(wishlists);
        }

        // POST /wishlist/:filmId
        [HttpPost("{filmId}")]
        [Authorize]
        public async Task<IActionResult> AddFilmToDefaultWishlist(int filmId)
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
                return Unauthorized("No se encontró el ID del usuario en el token.");

            var userId = int.Parse(claim.Value);
            var wishlistId = await _wishListRepository.GetDefaultWishlistIdAsync(userId);

            if (wishlistId is null)
                return NotFound("No se encontró la wishlist del usuario.");

            var success = await _wishListFilmRepository.AddFilmAsync(wishlistId.Value, filmId);
            if (!success)
                return BadRequest("No se pudo agregar la película.");

            return Ok();
        }

        // DELETE /wishlist/:filmId
        [HttpDelete("{filmId}")]
        [Authorize]
        public async Task<IActionResult> RemoveFilmFromDefaultWishlist(int filmId)
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
                return Unauthorized("No se encontró el ID del usuario en el token.");

            var userId = int.Parse(claim.Value);
            var wishlistId = await _wishListRepository.GetDefaultWishlistIdAsync(userId);

            if (wishlistId is null)
                return NotFound("No se encontró la wishlist del usuario.");

            var success = await _wishListFilmRepository.RemoveFilmAsync(wishlistId.Value, filmId);
            if (!success)
                return NotFound("La película no estaba en la wishlist.");

            return NoContent();
        }
    }
}