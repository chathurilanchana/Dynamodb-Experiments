using Microsoft.AspNetCore.Mvc;
using MovieRank.Domain;
using MovieRank.Modals;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("movies")]
    public class MovieRankController : ControllerBase
    {
        private readonly ILogger<MovieRankController> _logger;
        private readonly IMovieRankService _movieRankService;

        public MovieRankController(ILogger<MovieRankController> logger, IMovieRankService movieRankService)
        {
            _logger = logger;
            _movieRankService = movieRankService;
        }


        [HttpGet]
        public async Task<IEnumerable<MovieResponse>> GetAllMoviesAsync()
        {
            var results = await _movieRankService.GetAllMoviesFromDatabaseAsync();

            return results;
        }

        [HttpGet]
        [Route("user/{userId}/movie/{movieName}")]
        public async Task<MovieResponse> GetSpecificMovieAsync(int userId, string movieName)
        {
            var result = await _movieRankService.GetSpecificMovieAsync(userId, movieName);

            return result;
        }

        [HttpGet]
        [Route("user/{userId}/movieprefix/{prefix}")]
        public async Task<IEnumerable<MovieResponse>> GetMoviesBeginsWithForSpecificUserAsync(int userId, string prefix)
        {
            var result = await _movieRankService.GetMoviesBeginsWithAsyhnc(userId, prefix);

            return result;
        }

        [HttpPost]
        [Route("user/{userId}")]
        public async Task<IActionResult> PostMovieAsync(MovieRankRequest request)
        {
            await _movieRankService.AddMovieAsync(request);

            return Ok();
        }

        [HttpPatch] // use put if you modify the whole entity. Otherwise use patch
        [Route("user/{userId}")]
        public async Task<IActionResult> UpdateMovieAsync(int userId, UpdateMovieRankRequest updateRequest)
        {
            await _movieRankService.UpdateMovieAsync(userId, updateRequest);

            return Ok();
        }

        [HttpGet]
        [Route("/movie/{movieName}/ranking")]
        public async Task<double> GetOverallMovieRatingAsync(string movieName)
        {
            var result = await _movieRankService.GetRankingAsync(movieName);

            return result;
        }

        [HttpGet]
        [Route("/movie/{movieName}/descriptionprefix/{prefix}")]
        public async Task<double> GetOverallMovieRatingForDescriptionPrefixAsync(string movieName, string prefix)
        {
            var result = await _movieRankService.GetRankingForMoviesWithdescriptionPrefixAsync(movieName, prefix);

            return result;
        }
    }
}