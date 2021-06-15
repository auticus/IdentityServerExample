using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Movies.client.Models;

namespace Movies.client.Clients
{
    public class MovieAPIService : IMovieAPIService
    {
        public async Task<IEnumerable<Movie>> GetMovies()
        {
            var movies = new List<Movie>();
            movies.Add(new Movie
            {
                Id = 1,
                Genre = "Comics",
                Title = "Sammy Screams",
                Rating = "5.6",
                ImageUrl = "images/src",
                ReleaseDate = DateTime.Now,
                Owner = "auticus"
            });

            return await Task.FromResult(movies);
        }

        public Task<Movie> GetMovie(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Movie> CreateMovie(Movie movie)
        {
            throw new NotImplementedException();
        }

        public Task<Movie> UpdateMovie(Movie movie)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMovie(int id)
        {
            throw new NotImplementedException();
        }
    }
}
