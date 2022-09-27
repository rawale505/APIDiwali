namespace API_Diwali.Model
{
    public class NestedCartModel
    {
        public string _id { get; set; }//EmailId
        public List<CartItem> CartItems { get; set; }
        public double TotalProductCount { get; set; }
        public double BillAmount { get; set; }
    }
    public class CartItem
    {
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public double ProductPricePerItem { get; set; }
        public double ProductCount { get; set; }
        public string Image { get; set; }
    }
}
