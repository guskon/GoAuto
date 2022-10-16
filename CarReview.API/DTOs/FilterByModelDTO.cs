using CarReview.API.Models;
using CarReview.API.Validations;
using System.ComponentModel.DataAnnotations;

namespace CarReview.API.DTOs
{
    public class FilterByModelDTO
    {
        [Required]
        public string Model { get; set; }

        [EnsureMinimumElements(1, ErrorMessage = "At least one car is required!")]
        public List<GetCarDTO> FilteredCars { get; set; }
    }
}
