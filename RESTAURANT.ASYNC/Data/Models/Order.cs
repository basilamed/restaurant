namespace RESTAURANT.ASYNC.Data.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string OrderItems { get; set; }
        public string? PhoneNumber { get; set; }
        public string TotalPrice { get; set; }
    }
}
