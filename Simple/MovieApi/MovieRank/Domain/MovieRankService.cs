using MovieRank.Domain.Mappers;
using MovieRank.Infrastructure.Repositories;
using MovieRank.Modals;
using System.Linq;

namespace MovieRank.Domain
{
    public class MovieRankService : IMovieRankService
    {
        private readonly IMovieRankWithDocumentDbRepository _movieRankRepository;

        public MovieRankService(IMovieRankWithDocumentDbRepository movieRankRepository)
        {
            _movieRankRepository = movieRankRepository;
        }

        public async Task AddMovieAsync(MovieRankRequest request)
        {
            var mappedMovie = MovieRankDocumentDbMapper.ToDb(request);

            await _movieRankRepository.AddAsync(mappedMovie);
        }

        public async Task<IEnumerable<MovieResponse>> GetAllMoviesFromDatabaseAsync()
        {
            var result = await _movieRankRepository.GetAllItemsFromDatabaseAsync();

            return MovieRankDocumentDbMapper.ToApplication(result);
        }

        public async Task<IEnumerable<MovieResponse>> GetMoviesBeginsWithAsyhnc(int userId, string prefix)
        {
            var result = await _movieRankRepository.GetMoviesBeginsWithAsync(userId, prefix);

            return MovieRankDocumentDbMapper.ToApplication(result);
        }

        public async Task<double> GetRankingAsync(string movieName)
        {
            var result = await _movieRankRepository.GetAllRanksAsync(movieName);

            var overall = result.Average(p => p["Rank"].AsInt());

            return Math.Round(overall, 2);
        }

        public async Task<double> GetRankingForMoviesWithdescriptionPrefixAsync(string movieName, string prefix)
        {
            var result = await _movieRankRepository.GetAllRanksForMoviesWithDescriptionPrefixAsync(movieName,prefix);

            var overall = result.Average(p => Convert.ToInt32(p["Rank"]));

            return Math.Round(overall, 2);
        }

        public async Task<MovieResponse> GetSpecificMovieAsync(int userId, string movieName)
        {
            var result = await _movieRankRepository.GetMovieAsync(userId, movieName);

            return MovieRankDocumentDbMapper.ToApplication(result);
        }

        public async Task UpdateMovieAsync(int userId, UpdateMovieRankRequest request)
        {
            var original = await _movieRankRepository.GetMovieAsync(userId, request.MovieName);
            var mappedMovie = MovieRankDocumentDbMapper.ToDb(userId, original, request);

            await _movieRankRepository.UpdateAsync(mappedMovie);
        }
    }
}
