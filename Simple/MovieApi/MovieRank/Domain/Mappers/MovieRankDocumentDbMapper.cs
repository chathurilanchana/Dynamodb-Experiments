using Amazon.DynamoDBv2.DocumentModel;
using MovieRank.Infrastructure.Models;
using MovieRank.Modals;

namespace MovieRank.Domain.Mappers
{
    public class MovieRankDocumentDbMapper
    {
        public static IEnumerable<MovieResponse> ToApplication(IEnumerable<Document> items)
        {
            return items.Select(ToApplication);
        }

        public static MovieResponse ToApplication(Document item)
        {
            return new MovieResponse
            {
                UserId = item["UserId"].AsInt(),
                MovieName = item["MovieName"],
                Description = item["Description"],
                Rank = item["Rank"].AsInt(),
                RankedOn = Convert.ToDateTime(item["RankedOn"])
            };
        }

        public static Document ToDb(MovieRankRequest request)
        {
            var document = new Document();
            document["UserId"] = request.UserId;
            document["MovieName"] = request.MovieName;
            document["Description"] = request.Description;
            document["Rank"] = request.Rank;
            document["RankedOn"] = DateTime.Today.ToShortDateString();

            return document;
        }

        internal static Document ToDb(int userId, Document original, UpdateMovieRankRequest request)
        {

            var document = new Document();
            document["UserId"] = original["UserId"];
            document["MovieName"] = original["MovieName"];
            document["Description"] = original["Description"];
            document["Rank"] = request.Ranking;
            document["RankedOn"] = DateTime.Today.ToShortDateString();

            return document;
        }
    }
}
