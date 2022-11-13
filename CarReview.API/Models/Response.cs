using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarReview.API.Auth.Model;

namespace CarReview.API.Models
{
    public partial class Response : IUserOwnedResource
    {
        [Key]
        public int Id { get; set; }
        public int FkReviewId { get; set; }
        public int Status { get; set; }

        [Required]
        public string UserId { get; set; }
        //public CarReviewUser User { get; set; }

        [ForeignKey("FkReviewId")]
        [InverseProperty("Responses")]
        public virtual Review FkReview { get; set; } = null!;
    }
}
