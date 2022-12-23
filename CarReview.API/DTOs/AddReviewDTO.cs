using System.ComponentModel.DataAnnotations;

namespace CarReview.API.DTOs
{
    public class AddReviewDTO
    {
        [Required]
        public string Text { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Engine displacement has to be larger than 0!")]
        public float EngineDisplacement { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Engine power has to be larger than 0!")]
        public int EnginePower { get; set; }
        [Required]
        public string Positives { get; set; }
        [Required]
        public string Negatives { get; set; }
        [Required]
        [Range(1, 10, ErrorMessage = "Final score value has to be between 1 and 10!")]
        public int FinalScore { get; set; }

        [Required]
        public string Brand { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public string Generation { get; set; }
    }
}
