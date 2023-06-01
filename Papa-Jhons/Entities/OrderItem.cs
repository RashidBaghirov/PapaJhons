namespace Papa_Jhons.Entities
{
    public class OrderItem : BaseEntity
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public int? ProductId { get; set; }

        public int OrderId { get; set; }

        public Product Product { get; set; }

        public Order Order { get; set; }

        public User User { get; set; }

    }
}
