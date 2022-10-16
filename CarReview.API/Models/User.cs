using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarReview.API.Models
{
    public partial class User
    {
        public User()
        {
            Responses = new HashSet<Response>();
            Reviews = new HashSet<Review>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string UserName { get; set; } = null!;
        [StringLength(50)]
        [Unicode(false)]
        public string Password { get; set; } = null!;
        [StringLength(50)]
        [Unicode(false)]
        public string Email { get; set; } = null!;

        [InverseProperty("FkUser")]
        public virtual ICollection<Response> Responses { get; set; }
        [InverseProperty("FkUser")]
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
