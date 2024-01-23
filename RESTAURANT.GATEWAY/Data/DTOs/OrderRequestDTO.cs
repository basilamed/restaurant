namespace RESTAURANT.GATEWAY.Data.DTOs
{
    public class OrderRequestDTO
    {
        public string OrderItems { get; set; }
        public string PhoneNumber { get; set; }
        public string TotalPrice { get; set; }
    }
}
