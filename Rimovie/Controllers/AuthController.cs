using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rimovie.Entities;
using Rimovie.Models.Auth;
using Rimovie.Repository;
using Rimovie.Services.AuthService;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Rimovie.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        private readonly UserRepository _userRepository;
        private readonly WishListRepository _wishListRepository;

        public AuthController(IAuthService auth, UserRepository userRepository, WishListRepository wishListRepository)
        {
            _auth = auth;
            _userRepository = userRepository;
            _wishListRepository = wishListRepository;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
        {
            var user = await _auth.ValidateUserAsync(dto.Identifier, dto.Password);
            if (user == null) return Unauthorized();

            var result = await _auth.LoginAsync(user);
            SetRefreshCookie(result.RefreshToken);
            result.RefreshToken = null;
            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto dto)
        {
            var user = await _auth.RegisterAsync(dto);

            var userId = await _userRepository.InsertAsync(user);

            // Crear wishlist por defecto
            var defaultWishlist = new WishList
            {
                Name = "Mi lista",
                UserId = userId
            };
            await _wishListRepository.InsertAsync(defaultWishlist);
            var result = await _auth.LoginAsync(user);
            SetRefreshCookie(result.RefreshToken);
            result.RefreshToken = null;
            return Ok(result);
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<AuthResponseDto>> Refresh()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken)) return Unauthorized();

            var result = await _auth.RefreshTokensAsync(refreshToken);
            if (result == null) return Unauthorized();

            SetRefreshCookie(result.RefreshToken);
            result.RefreshToken = null;
            return Ok(result);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            await _auth.LogoutAsync(token);
            Response.Cookies.Delete("refreshToken");
            return NoContent();
        }

        [Authorize]
        [HttpPost("logout-all")]
        public async Task<IActionResult> LogoutAll()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _auth.LogoutAllAsync(userId);
            Response.Cookies.Delete("refreshToken");
            return NoContent();
        }

        private void SetRefreshCookie(string token)
        {
            Response.Cookies.Append("refreshToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            });
        }
    }
}
