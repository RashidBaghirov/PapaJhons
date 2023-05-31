using System.ComponentModel.DataAnnotations.Schema;

namespace Papa_Jhons.Entities
{
    public class Slider : BaseEntity
    {
        public string? ImagePath { get; set; }
        public byte? Order { get; set; }
        [NotMapped]

        public IFormFile? Image { get; set; }
    }
}
