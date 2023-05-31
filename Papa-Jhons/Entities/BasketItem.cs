namespace Papa_Jhons.Entities
{
    public class BasketItem : BaseEntity
    {
        public int ProductId { get; set; }

        public string UserId { get; set; }

        public int Count { get; set; }

        public Product Product { get; set; }

        public User User { get; set; }
    }
}
