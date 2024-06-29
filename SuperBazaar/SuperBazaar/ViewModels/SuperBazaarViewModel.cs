using SuperBazaar.Models;

namespace SuperBazaar.ViewModels
{
    public class SuperBazaarViewModel
    {
        public List<Item> Items { get; set; }
        public List<SelectedItem> SelectedItems { get; set; }
        public string SelectedItemName { get; set; }
        public int Quantity { get; set; }

    }
}
