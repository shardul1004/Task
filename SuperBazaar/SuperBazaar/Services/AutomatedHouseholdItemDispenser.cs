using Microsoft.Extensions.Caching.Memory;
using SuperBazaar.Models;

namespace SuperBazaar.Services
{
    public class AutomatedHouseholdItemDispenser: IAutomatedHouseholdItemDispenser
    {
        private readonly List<Item> Items;
        private readonly SuperBazarContext SBContext;
        public AutomatedHouseholdItemDispenser(SuperBazarContext SBContext)
        {
            this.SBContext = SBContext;
            string key = "items";
                Items = SBContext.Items.Where(item => true).ToList();
        }

        public List<Item> GetAvailableItems() { return Items; }

        public SelectedItem ProcessSelection(string itemName, int Quantity)
        {
            Item item = Items.Where(i => i.Itemname == itemName).FirstOrDefault();
            return new SelectedItem { ItemName=item.Itemname, Quantity=Quantity,TotalPrice = item.Itemprice*Quantity};
        }
    }
}
