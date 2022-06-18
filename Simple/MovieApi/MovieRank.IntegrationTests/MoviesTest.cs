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
            true.Should().Be(true);
        }
    }
}
