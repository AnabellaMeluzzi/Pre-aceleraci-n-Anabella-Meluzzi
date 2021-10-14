using DisneyAPI.Interfaces;
using DisneyAPI.ViewModel.Movie.Get;
using DisneyAPI.ViewModel.Movie.Post;
using DisneyAPI.ViewModel.Movie.Put;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DisneyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;
        public MovieController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        [HttpGet]
        [Route("movie/{id}")]
        public IActionResult GetMovie(int id)
        {
            try
            {
                var movie = _movieRepository.GetMovie(id);

                return Ok(movie);
            }
            catch (Exception ex)
            {
                if (ex.Message is "The movie is not in the database.") return NotFound(ex.Message);

                return BadRequest(ex.Message);
            }
        }        
        
        [HttpGet]
        [Route("movies")]
        public IActionResult GetAllMoviesVM()
        {
            var movies = _movieRepository.GetAllMoviesVm();

            if (movies is null)  return NoContent();

            return Ok(movies);
        }

        [HttpGet]
        [Route("movies-filtered")]
        public IActionResult GetFiltered([FromQuery] GetReqFilteredMovieVM model, bool asc)
        {
            try
            {
                var movies = _movieRepository.GetMovieByFilter(model, asc);
                return Ok(movies);
            }
            catch (Exception ex)
            {
                if (ex.Message is "The movie is not in the database." ||
                    ex.Message is "There is no movie with that genre in the database.") return NotFound(ex.Message);

                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post(PostReqMovieVM movieVM)
        {
            try
            {
                _movieRepository.AddMovie(movieVM);
                return Ok();
            }
            catch (Exception ex)
            {
                if (ex.Message is "The movie is already in the database.") return NotFound(ex.Message);

                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Put(PutReqMovieVM movieVM)
        {
            try
            {
                _movieRepository.EditMovie(movieVM);
                return Ok();
            }
            catch (Exception ex)
            {
                if (ex.Message is "The movie is not in the database.") return NotFound(ex.Message);

                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                _movieRepository.DeleteMovie(id);
                return Ok();
            }
            catch (Exception ex)
            {
                if (ex.Message is "The movie is not in the database.") return NotFound(ex.Message);

                return BadRequest(ex.Message);
            }
        }
    }
}
