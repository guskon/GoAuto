﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarReview.API.Auth.Model;
using Microsoft.EntityFrameworkCore;

namespace CarReview.API.Models
{
    public partial class Car : IUserOwnedResource
    {
        public Car()
        {
            Reviews = new HashSet<Review>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Brand { get; set; } = null!;
        [StringLength(50)]
        [Unicode(false)]
        public string Model { get; set; } = null!;
        [StringLength(50)]
        [Unicode(false)]
        public string Generation { get; set; } = null!;
        [Column(TypeName = "datetime")]
        public DateTime StartYear { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime EndYear { get; set; }

        [Required]
        public string UserId { get; set; }

        [InverseProperty("FkCar")]
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
