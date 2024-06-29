using Microsoft.Identity.Client;
using SuperBazaar.Models;

namespace SuperBazaar.Services
{
    public class Accounts:IAccounts
    {
        public int CalculateTotalCost(List<SelectedItem> SelectedItems)
        {
            int TotalCost = 0;
            foreach (var item in SelectedItems)
            {
                int itemprice = item.TotalPrice;
                TotalCost += itemprice;
            }
            return TotalCost;
        }


    }

    
}
