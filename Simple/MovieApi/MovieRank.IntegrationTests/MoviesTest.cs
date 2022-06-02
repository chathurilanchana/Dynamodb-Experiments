using MovieRank.Infrastructure.Models;
using MovieRank.IntegrationTests.Setup;
using MovieRank.Modals;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MovieRank.IntegrationTests
{
    // You can decorate this with collectionfixture if you want to initialize some services for multiple classes
    public class MoviesTest : IClassFixture<CustomWebApplicationFactory> // initialize things in customapplicationfactory(di) only once per this class
    {
        private static readonly System.Text.Json.JsonSerializerOptions Options = new System.Text.Json.JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true, 
        };
        readonly HttpClient _client;

        public MoviesTest(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task AddMovieRankData_ReturnsOk()
        {
            var statusCode = await AddMovie(1, "Titanic");

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task GetAllMovies()
        {
            await AddMovie(1, "Titanic");
            List<MovieResponse>? movies = null;

            //only reads headers and return. Data will be fetched as needed
            //https://code-maze.com/using-streams-with-httpclient-to-improve-performance-and-memory-usage/
            using (var response = await _client.GetAsync("movies", HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();

                //This will allow gc to clean content when exit using block. If content happen to be socket or memory stream this is critical
                // always dispose resources like sockets, memory streams etc
                using (var contentStream = await response.Content.ReadAsStreamAsync()) // When a class implement idisposible, we should dispose the object. Exception: Http client(dont do that, port exhaustion will happen)
                {
                    movies = await System.Text.Json.JsonSerializer.DeserializeAsync<List<MovieResponse>>(contentStream, Options);
                }
            }
            Assert.NotNull(movies);
            Assert.True(movies?.Any());
        }

        //We know the request we send is not huge. So we dont have to use a memory stream here
        private async Task<HttpStatusCode?> AddMovie(int userId, string movieName)
        {

            var movie1 = new MovieRankRequest
            {
                UserId = userId,
                MovieName = movieName,
                Description = $"This is description of movie {movieName}",
                Rank = 3
            };

            //var json = System.Text.Json.JsonSerializer.Serialize(movie1);
            //var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            //using (var result = await _client.PostAsync($"movies/user/{userId}", stringContent))

            var memoryStream = new MemoryStream();
            await System.Text.Json.JsonSerializer.SerializeAsync(memoryStream, movie1);
            memoryStream.Seek(0, SeekOrigin.Begin);
            var request = new HttpRequestMessage(HttpMethod.Post, $"movies/user/{userId}");
            HttpStatusCode? responseCode = null;
            using (var requestContent = new StreamContent(memoryStream))
            {
                request.Content = requestContent;
                requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                using (var response = await _client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    responseCode = response.StatusCode;
                }
            }

            return responseCode;
        }
    }
}
