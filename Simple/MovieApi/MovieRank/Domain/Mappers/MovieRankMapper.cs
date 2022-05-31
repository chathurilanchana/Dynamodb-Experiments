using MovieRank.Infrastructure.Models;
using MovieRank.Modals;

namespace MovieRank.Domain.Mappers
{
    public class MovieRankMapper
    {
        public static IEnumerable<MovieResponse> ToApplication(IEnumerable<MovieDb> movies)
        {
            return movies.Select(ToApplication);
        }

        public static MovieResponse ToApplication(MovieDb movie)
        {
            return new MovieResponse
            {
                UserId = movie.UserId,
                MovieName = movie.MovieName,
                Description = movie.Description,
                Rank = movie.Rank,
                RankedOn = Convert.ToDateTime(movie.RankedOn)            };
        }

        public static MovieDb ToDb(MovieRankRequest request)
        {
            return new MovieDb
            {
                UserId = request.UserId,
                MovieName = request.MovieName,
                Description = request.Description,
                Rank = request.Rank,
                RankedOn = DateTime.Today.ToShortDateString()
            };
        }

        internal static MovieDb ToDb(int userId, MovieDb original, UpdateMovieRankRequest request)
        {
            return new MovieDb
            {
                UserId = original.UserId,
                MovieName = original.MovieName,
                Description = original.Description,
                Rank = request.Ranking,
                RankedOn = DateTime.Today.ToShortDateString()
            };
        }
    }
}
