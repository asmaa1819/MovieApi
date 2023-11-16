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
    public class MoviesController : ControllerBase
    {
        
        private readonly IMovieServiecs _moviesServices;
        private readonly IGenreServices _genersServices;


        private new List<string> _allowedExtenstions = new List<string> { ".jpg", ".png" };
        private long _maxAllowedPosterSize = 1048576;

        public MoviesController(IMovieServiecs moviesServices, IGenreServices genersServices)
        {
            _moviesServices = moviesServices;
            _genersServices = genersServices;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {

            var movies = _moviesServices.GetAll();
            return Ok(movies);
        }


        [HttpGet(template:"{id}")]
        public async Task<IActionResult>GetByIdAsync(int id)
        {
            var movie = _moviesServices.GetById(id);
            if (movie == null)
                return NotFound();
            return Ok(movie);
        }
        [HttpPost]
        public async Task <IActionResult>CreateAsync([FromForm ]MovieDto dto)
        {
            if (!_allowedExtenstions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("Only .png and .jpg images are allowed!");
            if (dto.Poster.Length > _maxAllowedPosterSize)
                return BadRequest("Max allowed size for poster is 1MB!");

            var isValidGenre = await _genersServices.IsvalidGenre(dto.GenreId );
                

            if (!isValidGenre)
                return BadRequest("Invalid genere ID!");

            using var Datastream=new MemoryStream();
            await dto.Poster.CopyToAsync(Datastream);
            var movie = new Movie 
            {
                GenreId = dto.GenreId,
                Title = dto.Title,
                Poster = Datastream.ToArray(),
                Rate = dto.Rate,
                Storeline = dto.Storeline,  
                Year = dto.Year
            };
             _moviesServices.Add(movie);
            return Ok(movie);
        }
        [HttpPut(template:"{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromForm] MovieDto dto)
        {
            var movie = await _moviesServices.GetById(id);
            if (movie == null)
                return NotFound();

            var isValidGenre = await _genersServices.IsvalidGenre(dto.GenreId);

            if (!isValidGenre)
                return BadRequest("Invalid genere ID!");
            if (dto.Poster != null)
            {

                if (!_allowedExtenstions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                    return BadRequest("Only .png and .jpg images are allowed!");
                if (dto.Poster.Length > _maxAllowedPosterSize)
                    return BadRequest("Max allowed size for poster is 1MB!");

                using var Datastream = new MemoryStream();
                await dto.Poster.CopyToAsync(Datastream);
                movie.Poster = Datastream.ToArray();

            }

            movie.Title = dto.Title;
            movie.GenreId= dto.GenreId;
            movie.Year= dto.Year;
            movie.Rate= dto.Rate;
            movie.Storeline = dto.Storeline;

            _moviesServices.Update(movie);
            return Ok(movie);
            


        }
        [HttpDelete(template:"{id}")]
        public async Task <IActionResult>DeleteAsync(int id)
        {
            var movie = await _moviesServices.GetById(id);
            if (movie == null)
                return NotFound();
            _moviesServices.Delete(movie);
  
            return Ok(movie);

        }

    }
}
