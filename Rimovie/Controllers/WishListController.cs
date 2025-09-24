using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rimovie.Entities;
using Rimovie.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rimovie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListController : ControllerBase
    {
        private readonly WishListRepository _wishListRepository;
        public WishListController(WishListRepository wishListRepository)
        {
            _wishListRepository = wishListRepository;
        }

        [HttpGet("v1/all/{userId}")]
        public async Task<IActionResult> GetAllWishListByUser(long userId)
        {
            try
            {
                var response = await _wishListRepository.GetAllByUserAsync(userId);
                return Ok(response);
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }
}
