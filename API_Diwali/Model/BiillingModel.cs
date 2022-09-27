namespace API_Diwali.Model
{
    public class BiillingModel
    {
        public string _id { get; set; }//EmailId
        public List<CartItems> CartItems { get; set; }
    }
    public class CartItems
    {
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public double ProductPricePerItem { get; set; }
        public double ProductCount { get; set; }
        public string Image { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
