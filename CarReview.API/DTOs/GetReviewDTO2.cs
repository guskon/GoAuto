namespace CarReview.API.DTOs
{
    public class GetReviewDTO2
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string CreationDate { get; set; }
        public float EngineDisplacement { get; set; }
        public int EnginePower { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public string Positives { get; set; }
        public string Negatives { get; set; }
        public int FinalScore { get; set; }
        public string Username { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Generation { get; set; }
        public string UserId { get; set; }
    }
}
