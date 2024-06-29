namespace SuperBazaar.Models
{
    internal class Bill
    {
        public List<SelectedItem> Items { get; set; }
        public int TotalAmount { get; set; }
    }
}