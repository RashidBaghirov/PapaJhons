using System.ComponentModel.DataAnnotations.Schema;

namespace Papa_Jhons.Entities
{
    public class Offers : BaseEntity
    {
        public string? ImagePath { get; set; }
        [NotMapped]

        public IFormFile? Image { get; set; }
    }
}
