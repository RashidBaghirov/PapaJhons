using Papa_Jhons.Entities;
using System.ComponentModel.DataAnnotations;

namespace Papa_Jhons.ViewModel
{
    public class OrderVm
    {
        [Required]
        [StringLength(maximumLength: 60)]
        public string Fullname { get; set; }
        [Required]
        [StringLength(maximumLength: 20)]
        public string Username { get; set; }
        [Required]
        [StringLength(maximumLength: 60)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [StringLength(maximumLength: 100)]
        public string Address { get; set; }
        [Required]
        [StringLength(maximumLength: 100)]
        public string Message { get; set; }
        [DataType(DataType.PhoneNumber)]
        public int Number { get; set; }
        [Required]
        [StringLength(maximumLength: 100)]
        public string City { get; set; }
        public List<BasketItem>? BasketItems { get; set; }
    }
}
