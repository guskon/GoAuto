using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarReview.API.Models
{
    public partial class Response
    {
        [Key]
        public int Id { get; set; }
        public int FkUserId { get; set; }
        public int FkReviewId { get; set; }
        public int Status { get; set; }

        [ForeignKey("FkReviewId")]
        [InverseProperty("Responses")]
        public virtual Review FkReview { get; set; } = null!;
        [ForeignKey("FkUserId")]
        [InverseProperty("Responses")]
        public virtual User FkUser { get; set; } = null!;
    }
}
