using Microsoft.AspNetCore.Mvc;
using SuperBazaar.Models;
using SuperBazaar.Services;
using SuperBazaar.ViewModels;
using System.Text.Json;

namespace SuperBazaar.Controllers
{
    public class HomePageController : Controller
    {
        private readonly ISuperBazar SuperBazar;

        public HomePageController(ISuperBazar SuperBazar, SuperBazarContext SBContext)
        {
            this.SuperBazar = SuperBazar;
        }

        public IActionResult HomePageView(SuperBazaarViewModel superBazaarViewModel)
        {
            superBazaarViewModel.Items = SuperBazar.GetAvailableItems();
            var cartJson = HttpContext.Session.GetString("Cart");
            superBazaarViewModel.SelectedItems = string.IsNullOrEmpty(cartJson)
                ? new List<SelectedItem>()
                : JsonSerializer.Deserialize<List<SelectedItem>>(cartJson);
            return View("HomePageView", superBazaarViewModel);
        }

        public IActionResult RemoveItem(string SelectedItemName)
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            var cart = string.IsNullOrEmpty(cartJson)
                ? new List<SelectedItem>()
                : JsonSerializer.Deserialize<List<SelectedItem>>(cartJson);

            var itemToRemove = cart.FirstOrDefault(item => item.ItemName == SelectedItemName);
            if (itemToRemove != null)
            {
                cart.Remove(itemToRemove);
                HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));
            }

            SuperBazaarViewModel model = new SuperBazaarViewModel();
            model.SelectedItems = cart;
            return HomePageView(model);
        }

        public IActionResult AddItem(string SelectedItemName, int Quantity)
        {
            var selectedItem = SuperBazar.AddItemToCart(SelectedItemName, Quantity);
            int price = selectedItem.TotalPrice / selectedItem.Quantity;

            var cartJson = HttpContext.Session.GetString("Cart");
            var cart = string.IsNullOrEmpty(cartJson)
                ? new List<SelectedItem>()
                : JsonSerializer.Deserialize<List<SelectedItem>>(cartJson);

            var existingItem = cart.FirstOrDefault(item => item.ItemName == SelectedItemName);

            if (existingItem != null)
            {
                existingItem.Quantity += selectedItem.Quantity;
                existingItem.TotalPrice = existingItem.Quantity * price;
            }
            else
            {
                cart.Add(selectedItem);
            }

            HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));

            SuperBazaarViewModel model = new SuperBazaarViewModel();
            model.SelectedItems = cart;
            return HomePageView(model);
        }

        public async Task<IActionResult> Buy(SuperBazaarViewModel model)
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            var cart = string.IsNullOrEmpty(cartJson)
                ? new List<SelectedItem>()
                : JsonSerializer.Deserialize<List<SelectedItem>>(cartJson);

            if (cart == null || !cart.Any())
            {
                ModelState.AddModelError(string.Empty, "No items selected for purchase.");
                return HomePageView(model);
            }

            var status = await SuperBazar.BuySelectedItems(cart);
            if (status.Success)
            {
                int totalCost = SuperBazar.CalculateTotalCost(cart);
                var bill = new Bill
                {
                    Items = cart,
                    TotalAmount = totalCost
                };

                HttpContext.Session.Remove("Cart");

                return View("Buy", bill);
            }
            else
            {
                ModelState.AddModelError(string.Empty, status.Message);
                return HomePageView(model);
            }
        }
    }
}