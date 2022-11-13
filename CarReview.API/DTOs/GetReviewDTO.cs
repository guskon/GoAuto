namespace CarReview.API.DTOs
{
    public class GetReviewDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }
        public float EngineDisplacement { get; set; }
        public int EnginePower { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public string Positives { get; set; }
        public string Negatives { get; set; }
        public int FinalScore { get; set; }
        public string UserId { get; set; }
        public int FkCarId { get; set; }
    }
}
