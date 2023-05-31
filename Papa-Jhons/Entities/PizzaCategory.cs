namespace Papa_Jhons.Entities
{
    public class PizzaCategory : BaseEntity
    {
        public string Name { get; set; }
        public List<Product> Product { get; set; }
    }
}
