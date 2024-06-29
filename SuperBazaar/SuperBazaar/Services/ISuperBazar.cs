using SuperBazaar.Models;

namespace SuperBazaar.Services
{
    public interface ISuperBazar
    {
        public List<Item> GetAvailableItems();
        public SelectedItem AddItemToCart(string itemName, int quantity);
        public int CalculateTotalCost(List<SelectedItem> selectedItems);
        public Task<Status> BuySelectedItems(List<SelectedItem> selectedItems);
    }
}
