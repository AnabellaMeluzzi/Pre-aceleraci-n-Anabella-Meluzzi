using DisneyAPI.Context;
using DisneyAPI.Entities;
using DisneyAPI.Interfaces;
using DisneyAPI.ViewModel.Movie.Get;
using DisneyAPI.ViewModel.Movie.Post;
using DisneyAPI.ViewModel.Movie.Put;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DisneyAPI.Repositories
{
    public class MovieRepository : BaseRepository<Movie, DisneyContext>, IMovieRepository
    {
        public MovieRepository(DisneyContext dbContext) : base(dbContext)
        {

        }
        public Movie GetMovieEntity(int id)
        {
            return DbSet.Include(x => x.Characters).Include(x => x.Genres).ToList().FirstOrDefault(x => x.Id == id);
        }

        public GetResMovieDetailVM GetMovie(int id)
        {
            var movie = DbSet.Include(m => m.Genres).ToList().FirstOrDefault(x => x.Id == id);

            if (movie is default(Movie)) throw new Exception("The movie is not in the database.");

            List<string> genres = new List<string>();
            genres = CheckGenresNames(movie);
            List<string> characters = new List<string>();
            characters = CheckCharactersNames(movie);

            GetResMovieDetailVM characterVM = new GetResMovieDetailVM
            {
                Image = movie.Image,
                Title = movie.Title,
                CreationDate = movie.CreationDate,
                Score = movie.Score,
                Characters = characters,
                Genres = genres
            };

            return characterVM;
        }

        public List<GetResMovieVM> GetAllMoviesVm()
        {
            var moviesList = DbSet.ToList();

            List<GetResMovieVM> moviesVM = new List<GetResMovieVM>();
            foreach (var movie in moviesList)
            {
                moviesVM.Add(new GetResMovieVM
                {
                    Image = movie.Image,
                    Title = movie.Title,
                    CreationDate = movie.CreationDate
                });
            }
            return (moviesVM);
        }

        public List<GetResFilteredMovieVM> GetMovieByFilter(GetReqFilteredMovieVM model, bool desc)
        {
            var queryMovies = DbSet.Include(x => x.Genres).ToList().Where(g => g.Title.Contains(model.Title)).ToList();
            if (queryMovies.Count < 1) throw new Exception("The movie is not in the database.");

            try
            {
                if (model.Genre is not null)
                {
                    List<Movie> moviesToShow = new List<Movie>();
                    foreach (var movie in queryMovies)
                    {
                        var query = (from genresInMovie in movie.Genres
                                    join genre in _dbContext.Genres on genresInMovie.Name equals genre.Name
                                    where model.Genre == genresInMovie.Name
                                    select movie).ToList();
                        if(query.Count != 0)
                        {
                            moviesToShow = query;
                        }
                    }
                    
                    if (moviesToShow.Count == 0) throw new Exception("There is no movie with that genre in the database.");
                    queryMovies = moviesToShow;
                }

                if (desc == true) queryMovies = queryMovies.OrderByDescending(x => x.CreationDate).ToList();

                List<GetResFilteredMovieVM> movieVM = new List<GetResFilteredMovieVM>();
                foreach (var movie in queryMovies)
                {
                    movieVM.Add(new GetResFilteredMovieVM
                    {
                        Image = movie.Image,
                        Title = movie.Title,
                        CreationDate = movie.CreationDate,
                        Score = movie.Score
                    });
                }
                return (movieVM);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddMovie(PostReqMovieVM movieVM)
        {
            var movieInDB = DbSet.FirstOrDefault(x => x.Title == movieVM.Title);
            if (movieInDB is not default(Movie)) throw new Exception("The movie is already in the database.");

            try
            {
                Movie movie = new Movie
                {
                    Title = movieVM.Title,
                    Image = movieVM.Image,
                    CreationDate = movieVM.CreationDate,
                    Score = movieVM.Score,
                };

                Add(movie);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void EditMovie(PutReqMovieVM movieVM)
        {
            var movieInDB = DbSet.Include(x => x.Genres).Include(x => x.Characters).ToList().FirstOrDefault(x => x.Id == movieVM.Id);
            if (movieInDB is default(Movie)) throw new Exception("The movie is not in the database.");

            List<Character> character = new List<Character>();
            character = (movieVM.CharactersIDs is not null) ? _dbContext.Characters.Where(c => c.Id == movieVM.CharactersIDs).ToList()
                                                            : null;            
            
            List<Genre> genre = new List<Genre>();
            genre = (movieVM.GenresIDs is not null) ? _dbContext.Genres.Where(m => m.Id == movieVM.GenresIDs).ToList()
                                                    : null;

            movieInDB.Title = movieVM.Title;
            movieInDB.Image = movieVM.Image;
            movieInDB.CreationDate = movieVM.CreationDate;
            movieInDB.Score = movieVM.Score;
            movieInDB.Characters = character;
            movieInDB.Genres = genre;

            try
            {
                Put(movieInDB);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteMovie(int id)
        {
            var movieInDB = GetMovieEntity(id);
            if (movieInDB is default(Movie)) throw new Exception("The movie is not in the database.");

            try
            {
                Delete(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        private List<string> CheckGenresNames(Movie movie)
        {
            List<string> genres; 
            if (movie.Genres is null) return new List<string> { "No genres associated" };

            genres = new List<string>();
            foreach(var genre in movie.Genres)
            {
                genres.Add(genre.Name);
            }
            return genres;
        }

        private List<string> CheckCharactersNames(Movie movie)
        {
            List<string> characters;
            if (movie.Characters is null) return new List<string> { "No characters associated" };

            characters = new List<string>();
            foreach (var charac in movie.Characters)
            {
                characters.Add(charac.Name);
            }

            return characters;
        }
    }
}
