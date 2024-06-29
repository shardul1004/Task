using SuperBazaar.Models;

namespace SuperBazaar.Services
{
    public interface IAutomatedHouseholdItemDispenser
    {
        public List<Item> GetAvailableItems();
        public SelectedItem ProcessSelection(string itemName, int Quantity);
    }
}
