using System.ComponentModel.DataAnnotations;

namespace CarReview.API.DTOs
{
    public class GetCarDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public string Generation { get; set; }
        [Required]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "Start year has to be exactly 4 digits!")]
        public string StartYear { get; set; }
        [Required]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "End year has to be exactly 4 digits!")]
        public string EndYear { get; set; }
        public string UserId { get; set; }
    }
}
