using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rimovie.Entities;
using Rimovie.Repository.Interfaces;
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
        private IGenericRepository<Film> _repository;
        public FilmsController(IGenericRepository<Film> repository)
        {
            _repository = repository;
        }

        [HttpGet("v1/all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var response = await _repository.GetAllAsync();
                return Ok(response);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
