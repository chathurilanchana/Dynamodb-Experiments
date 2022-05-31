using Amazon.DynamoDBv2.DataModel;

namespace MovieRank.Infrastructure.Models
{
    [DynamoDBTable("Movies")]
    public class MovieDb
    {
        [DynamoDBHashKey]
        public int UserId { get; set; } 

        public string MovieName { get; set; }
        public string Description { get; set; }
        public int Rank{ get; set; }
        public string RankedOn{ get; set; }
    }
}
