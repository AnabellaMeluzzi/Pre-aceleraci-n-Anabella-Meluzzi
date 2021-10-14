using DisneyAPI.Entities;
using DisneyAPI.ViewModel.Character.Get;
using DisneyAPI.ViewModel.Movie.Get;
using DisneyAPI.ViewModel.Movie.Post;
using DisneyAPI.ViewModel.Movie.Put;
using System.Collections.Generic;

namespace DisneyAPI.Interfaces
{
    public interface IMovieRepository
    {
        Movie GetMovieEntity(int id);

        GetResMovieDetailVM GetMovie(int id);

        List<GetResMovieVM> GetAllMoviesVm();

        List<GetResFilteredMovieVM> GetMovieByFilter(GetReqFilteredMovieVM model, bool asc);

        void AddMovie(PostReqMovieVM movieVM);

        void EditMovie(PutReqMovieVM movieVM);

        void DeleteMovie(int id);
    }
}
