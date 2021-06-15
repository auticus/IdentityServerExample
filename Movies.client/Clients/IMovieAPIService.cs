using System.Collections.Generic;
using System.Threading.Tasks;
using Movies.client.Models;

namespace Movies.client.Clients
{
    public interface IMovieAPIService
    {
        Task<IEnumerable<Movie>> GetMovies();
        Task<Movie> GetMovie(string id);
        Task<Movie> CreateMovie(Movie movie);
        Task<Movie> UpdateMovie(Movie movie);
        Task DeleteMovie(int id);
    }
}
