namespace CarReview.API.DTOs
{
    public class GetResponsesDTO
    {
        public int Id { get; set; }
        public int FkReviewId { get; set; }
        public int Status { get; set; }
        public string UserId { get; set; }

    }
}
