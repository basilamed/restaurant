using System.ComponentModel.DataAnnotations;

namespace RESTAURANT.GATEWAY.Data.DTOs
{
    public class OrderDTO
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public List<string> OrderItems { get; set; }
        public string PhoneNumber { get; set; }
        public string TotalPrice { get; set; }
    }
}
