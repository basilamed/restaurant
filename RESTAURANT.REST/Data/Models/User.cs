using Microsoft.AspNetCore.Identity;

namespace RESTAURANT.REST.Data.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public int RoleId { get; set; }
    }
}
