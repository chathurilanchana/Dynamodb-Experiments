namespace MovieRank.Modals
{
    public class MovieResponse
    {
        public int UserId { get; set; }
        public string MovieName { get; set; }
        public string Description { get; set; }
        public int Rank { get; set; }
        public DateTime RankedOn { get; set; }
    }
}
