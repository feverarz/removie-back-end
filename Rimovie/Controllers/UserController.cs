using Microsoft.AspNetCore.Mvc;
using Rimovie.Mappers;
using Rimovie.Models.Request;
using Rimovie.Repository;
using System.Threading.Tasks;

namespace Rimovie.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("by-email")]
        public async Task<IActionResult> GetByEmail([FromQuery] string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user is null)
                return NotFound("Usuario no encontrado.");

            var dto = UserMapper.ToResponseDto(user);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserCreateDto dto)
        {
            var user = UserMapper.ToEntity(dto);
            var userId = await _userRepository.InsertAsync(user);
            user.UserId = userId;

            var response = UserMapper.ToResponseDto(user);
            return Ok(response);
        }
    }
}