using CarReview.API.Models;
using CarReview.API.Validations;
using System.ComponentModel.DataAnnotations;

namespace CarReview.API.DTOs
{
    public class FilterByGenerationDTO
    {
        [Required]
        public string Generation { get; set; }

        [EnsureMinimumElements(1, ErrorMessage = "At least one car is required!")]
        public List<GetCarDTO> FilteredCars { get; set; }
    }
}
