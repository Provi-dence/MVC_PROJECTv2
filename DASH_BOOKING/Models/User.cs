using System;
using System.ComponentModel.DataAnnotations;

namespace DASH_BOOKING.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        public DateTime CreatedAt { get; set; }

        public User()
        {
            CreatedAt = DateTime.Now;
        }
    }

    public class Admin : User
    {
        [Required]
        public string Role { get; set; } = "Admin";

        public Admin() : base()
        {
        }
    }
}
