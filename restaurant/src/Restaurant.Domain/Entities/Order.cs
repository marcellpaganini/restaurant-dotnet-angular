namespace Restaurant.Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; } = "Pending";
    public decimal TotalAmount { get; set; }

    public Client? Client { get; set; }
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}
