namespace Papa_Jhons.Entities
{
    public class Order : BaseEntity
    {
        public string City { get; set; }

        public string Address { get; set; }

        public double TotalPrice { get; set; }

        public bool? Status { get; set; }
        public string? Message { get; set; }
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }

        public User User { get; set; }

        public List<OrderItem>? OrderItems { get; set; }
    }
}
