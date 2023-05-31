using Papa_Jhons.Entities;

namespace Papa_Jhons.ViewModel
{
    public class BasketVm
    {
        public List<BasketItem> BasketItems { get; set; }
        public decimal TotalPrice { get; set; }
        public int Count { get; set; }
    }
}
