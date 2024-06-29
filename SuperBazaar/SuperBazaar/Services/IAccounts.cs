using SuperBazaar.Models;

namespace SuperBazaar.Services
{
    public interface IAccounts
    {
        public int CalculateTotalCost(List<SelectedItem> selectedItems);
    }
}
