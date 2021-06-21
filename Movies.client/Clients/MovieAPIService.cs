using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Movies.client.Models;
using Newtonsoft.Json;

namespace Movies.client.Clients
{
    public class MovieAPIService : IMovieAPIService
    {
        private readonly IHttpClientFactory _clientFactory;

        public MovieAPIService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IEnumerable<Movie>> GetMovies()
        {
            //going to use an interceptor to insert the token as needed
            var client = _clientFactory.CreateClient("MoviesAPIClient");
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/movies/");
            var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                .ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var rawContent = await response.Content.ReadAsStringAsync();

            //deserialize object to the movieList
            var movies = JsonConvert.DeserializeObject<List<Movie>>(rawContent);
            return movies;
        }
        /*
         * This demonstrates a bad way to consume identity server, but it works
         *
        public async Task<IEnumerable<Movie>> GetMovies()
        {
            // get token from identity server
            var apiClientCredentials = new ClientCredentialsTokenRequest()
            {
                Address = "https://localhost:5005/connect/token", //hard-coded badness 
                ClientId = "moviesClient",
                ClientSecret = "smeagolCries",
                Scope = "moviesAPI"
            };

            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5005");
            if (disco.IsError) return null;  //throws 500 error

            var response = await client.RequestClientCredentialsTokenAsync(apiClientCredentials);
            if (response.IsError) return null;


            // send request to Protected API
            var moviesAPI = new HttpClient();
            moviesAPI.SetBearerToken(response.AccessToken);

            var moviesResponse = await moviesAPI.GetAsync("https://localhost:5001/api/movies");
            moviesResponse.EnsureSuccessStatusCode();
            var rawContent = await moviesResponse.Content.ReadAsStringAsync();

            //deserialize object to the movieList
            var movies = JsonConvert.DeserializeObject<List<Movie>>(rawContent);
            return movies;
        }
        */

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
