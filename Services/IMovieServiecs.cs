﻿using MoviesApi.Models;

namespace MoviesApi.Services
{
    public interface IMovieServiecs
    {
        Task<IEnumerable<Movie>> GetAll();
        Task<Movie> GetById(int id);
        Task<Movie> Add(Movie movie);
        Movie Update(Movie movie);
        Movie Delete(Movie movie);




    }
}
