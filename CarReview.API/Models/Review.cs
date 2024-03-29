﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarReview.API.Auth.Model;
using Microsoft.EntityFrameworkCore;

namespace CarReview.API.Models
{
    public partial class Review : IUserOwnedResource
    {
        public Review()
        {
            Responses = new HashSet<Response>();
        }

        [Key]
        public int Id { get; set; }
        [Column(TypeName = "text")]
        public string Text { get; set; } = null!;
        [Column(TypeName = "datetime")]
        public DateTime CreationDate { get; set; }
        public float EngineDisplacement { get; set; }
        public int EnginePower { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Positives { get; set; } = null!;
        [StringLength(50)]
        [Unicode(false)]
        public string Negatives { get; set; } = null!;
        public int FinalScore { get; set; }
        public int FkCarId { get; set; }

        [Required]
        public string UserId { get; set; }
        //public CarReviewUser User { get; set; }

        [ForeignKey("FkCarId")]
        [InverseProperty("Reviews")]
        public virtual Car FkCar { get; set; } = null!;
        
        [InverseProperty("FkReview")]
        public virtual ICollection<Response> Responses { get; set; }
    }
}
