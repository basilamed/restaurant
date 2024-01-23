using Microsoft.AspNetCore.Identity;
using RESTAURANT.REST.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace RESTAURANT.REST.Data.DTOs
{
    public class UserRegisterDTO
    {
        [Required]
        [StringLength(30, MinimumLength = 5)]
        public string Email { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 8)]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string LastName { get; set; }
        [Required]
        [StringLength(10, MinimumLength = 9)]
        public string PhoneNumber { get; set; }
        [Required]
        public Role Role { get; set; }
    }
}
