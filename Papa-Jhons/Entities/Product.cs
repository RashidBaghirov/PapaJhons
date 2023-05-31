using System.ComponentModel.DataAnnotations.Schema;

namespace Papa_Jhons.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? Desc { get; set; }

        public string ImagePath { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int? PizzaCategoryId { get; set; }
        public PizzaCategory? PizzaCategory { get; set; }



        [NotMapped]

        public IFormFile Image { get; set; }



    }
}
