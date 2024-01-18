using System.ComponentModel.DataAnnotations;

namespace RESTAURANT.REST.Data.DTOs
{
    public class UserLoginDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
