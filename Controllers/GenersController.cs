using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Dtos;
using MoviesApi.Models;
using MoviesApi.Services;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenersController : ControllerBase
    {
        private readonly IGenreServices _genreServices;

        public GenersController( IGenreServices genreServices)
        {
            _genreServices = genreServices;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var geners = await _genreServices.GetAll();
            return Ok(geners);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreatGenerDto dto)
        {
            Genre genre = new() { Name = dto.Name };
            await _genreServices.Add(genre);
            return Ok(genre);


             
        }
        [HttpPut(template: "{id}")]
        public async Task<IActionResult> UpdateAsync(byte id, [FromBody] CreatGenerDto dto)
        {
            var gener = await _genreServices.GetById(id);

            if (gener== null)
            return NotFound($"No Gener With Id:{id}");

             gener.Name=dto.Name;
            _genreServices.Update(gener);
             return Ok(gener);
        }
        [HttpDelete(template: "{id}")]
        public async Task<IActionResult>DeleteAsync(byte id)
        {
            
            var gener = await _genreServices.GetById(id);

            if (gener == null)
                return NotFound($"No Gener With Id:{id}");
            _genreServices.Delete(gener);
            return Ok(gener);

        }
    }
}
