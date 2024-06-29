using System.Buffers;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SuperBazaar.Models;

namespace SuperBazaar.Services
{
    public class SuperBazar: ISuperBazar
    {
        private readonly IAutomatedHouseholdItemDispenser dispenser;
        private readonly IAccounts accounts;
        private readonly SuperBazarContext SbContext;
        public SuperBazar(IAutomatedHouseholdItemDispenser dispenser, IAccounts accounts, SuperBazarContext SbContext)
        {
            this.dispenser = dispenser;
            this.accounts = accounts;
            this.SbContext = SbContext;
        }

        public List<Item> GetAvailableItems()
        {
            return dispenser.GetAvailableItems();
        }

        public SelectedItem AddItemToCart(string itemName, int quantity)
        {
            return dispenser.ProcessSelection(itemName, quantity);
        }

        public int CalculateTotalCost(List<SelectedItem> selectedItems)
        {
            return accounts.CalculateTotalCost(selectedItems);
        }

        public async Task<Status> BuySelectedItems(List<SelectedItem> selectedItems)
        {
            using (var transaction = await SbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    foreach (var selectedItem in selectedItems)
                    {
                        var item = await SbContext.Items
                            .FirstOrDefaultAsync(i => i.Itemname == selectedItem.ItemName);

                        if (item == null)
                        {
                            return new Status
                            {
                                Success = false,
                                Message = $"Item {selectedItem.ItemName} not found in the database."
                            };
                        }

                        if (item.Itemquantity < selectedItem.Quantity)
                        {
                            return new Status
                            {
                                Success = false,
                                Message = $"Insufficient quantity for {selectedItem.ItemName}. Available: {item.Itemquantity}, Requested: {selectedItem.Quantity}"
                            };
                        }

                        item.Itemquantity -= selectedItem.Quantity;
                        SbContext.Items.Update(item);
                        
                    }
                    await SbContext.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new Status
                    {
                        Success = true,
                        Message = "Items purchased successfully."
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return new Status
                    {
                        Success = false,
                        Message = $"An error occurred: {ex.Message}"
                    };
                }
            }
        }

    }
}
