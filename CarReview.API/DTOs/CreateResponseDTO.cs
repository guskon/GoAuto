using System.ComponentModel.DataAnnotations;

namespace CarReview.API.DTOs
{
    public class CreateResponseDTO
    {
        [Required]
        public int FkUserId { get; set; }
        [Required]
        public int FkReviewId { get; set; }
        [Required]
        [Range(0, 1, ErrorMessage = "Status has to be 1 or 0!")]
        public int Status { get; set; }
    }
}
